﻿using JobScheduler.Controllers;
using JobScheduler.Shared.Models;
using Microsoft.Extensions.Logging;
using NCrontab.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Timers;
using Timer = System.Timers.Timer;

namespace JobScheduler.BackgroundWorker
{
    /// <summary>
    /// Manages jobs queue
    /// </summary>
    public class JobsScheduler
    {
        //Jobs list ordered by time
        public List<Schedule> Jobs = new List<Schedule>();
        private Timer WakeUpTimer;
        private readonly Timer UpdateJobsTimer;
        private readonly ILogger<JobsScheduler> _logger;
        private readonly SchedulesMethods _schedulesMethods;
        private readonly JobRunner _jobRunner;
        private readonly GroupsMethods _groupsMethods;
        private readonly NodesMethods _nodesMethods;

        public JobsScheduler(SchedulesMethods schedulesMethods, ILogger<JobsScheduler> logger, JobRunner jobRunner, GroupsMethods groupsMethods, NodesMethods nodesMethods)
        {
            _schedulesMethods = schedulesMethods;
            _logger = logger;
            _jobRunner = jobRunner;
            _groupsMethods = groupsMethods;
            _nodesMethods = nodesMethods;

            PopulateJobsQueue();

            //Updates the Jobs list every minute
            UpdateJobsTimer = new Timer
            {
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds
            };
            UpdateJobsTimer.Elapsed += (object sender, ElapsedEventArgs e) => PopulateJobsQueue();
            UpdateJobsTimer.Start();
        }

        /// <summary>
        /// Updates the queue
        /// </summary>
        private async void PopulateJobsQueue()
        {
            if (Jobs == null)
                Jobs = new List<Schedule>();

            Jobs.Clear();

            foreach (Schedule schedule in await _schedulesMethods.GetSchedulesAsync())
            {
                if (schedule.Job != null)
                {
                    schedule.When = CrontabSchedule.Parse(schedule.Cron).GetNextOccurrence(DateTime.Now);
                    AddJob(schedule);
                }
            }

            Jobs = Jobs.OrderBy(x => x.When).ToList();

            UpdateWakeUpTimer();
        }

        /// <summary>
        /// Sets the timer when the JobRunner should "wake up" and run the scheduled job.
        /// </summary>
        private void UpdateWakeUpTimer()
        {
            if (WakeUpTimer != null)
                WakeUpTimer.Elapsed -= WakeUp;
            WakeUpTimer = new Timer();

            if (Jobs.Count == 0)
                return;

            DateTime nextjob = Jobs.FirstOrDefault().When;
            double time = (nextjob - DateTime.Now).TotalMilliseconds;
            if (time <= 0)
            {
                WakeUp(null, null);
            }
            else
            {
                WakeUpTimer.Interval = time;
                WakeUpTimer.Elapsed += WakeUp;
                //Execute only once
                WakeUpTimer.AutoReset = false;
                WakeUpTimer.Start();
            }
        }

        /// <summary>
        /// Runs the first job in the queue and updates the queue
        /// </summary>
        private async void WakeUp(object sender, ElapsedEventArgs e)
        {
            Job job = Jobs?.FirstOrDefault()?.Job;
            if (job != null)
            {
                //Get available groups
                IEnumerable<Group> groups = await _groupsMethods.GetGroupsAsync();
                //Group id
                int? groupId = job.GroupId;

                if (groupId == null)
                {
                    //Run job locally
                    await _jobRunner.ExecuteAsync(Jobs.FirstOrDefault()?.Job);

                    foreach (Node node in await _nodesMethods.GetNodesAsync())
                        RunJobOnNodes(node, job);
                }
                else
                {
                    IEnumerable<Node> nodes = groups.FirstOrDefault(x => x.Id == groupId).GroupNodes.Select(x => x.Node);
                    foreach (Node node in nodes)
                    {
                        if (node.Role == NodeRole.Master)
                        {
                            //Run job locally
                            await _jobRunner.ExecuteAsync(Jobs.FirstOrDefault()?.Job);
                        }
                        else
                        {
                            RunJobOnNodes(node, job);
                        }
                    }
                }
            }
            RemoveExecutedJob();
            UpdateWakeUpTimer();
        }

        private async void RunJobOnNodes(Node node, Job job)
        {
            try
            {
                using HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync($"{node.IPStr}:{node.Port}/api/jobs/start", content);
                if (res.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Job accepted by slave");
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Removes the first job in the queue
        /// </summary>
        private void RemoveExecutedJob()
        {
            try
            {
                Schedule job = Jobs.FirstOrDefault();
                Jobs.Remove(job);

                if (job == null)
                    return;

                job.When = CrontabSchedule.Parse(job.Cron).GetNextOccurrence(DateTime.Now);
                Jobs.Add(job);
                Jobs = Jobs.OrderBy(x => x.When).ToList();
            }
            catch { }
        }

        /// <summary>
        /// Adds a Job to the queued jobs
        /// </summary>
        /// <param name="schedule">The schedule to add</param>
        public void AddJob(Schedule schedule)
        {
            try
            {
                //Fail silently if no cron available
                if (string.IsNullOrEmpty(schedule.Cron))
                    return;

                schedule.When = CrontabSchedule.Parse(schedule.Cron).GetNextOccurrence(DateTime.Now);
                Jobs.Add(schedule);
                _logger.LogInformation($"Added job {schedule.Id} - {schedule.Cron}");
            }
            catch
            {
                //item with same job error
            }
        }

        /// <summary>
        /// Schedules a task to be run now (ASAP)
        /// </summary>
        /// <param name="schedule"></param>
        public void StartJobNow(Schedule schedule)
        {
            Jobs.Insert(0, schedule);
            WakeUp(null, null);
            _logger.LogInformation($"Added job {schedule.Id}");
        }
    }
}
