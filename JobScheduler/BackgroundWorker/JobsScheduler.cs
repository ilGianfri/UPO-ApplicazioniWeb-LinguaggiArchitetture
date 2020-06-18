using JobScheduler.Controllers;
using JobScheduler.Shared.Models;
using Microsoft.Extensions.Logging;
using NCrontab.Advanced;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public SortedList<DateTime, Schedule> Jobs = new SortedList<DateTime, Schedule>();
        private Timer WakeUpTimer;
        private readonly Timer UpdateJobsTimer;
        private readonly ILogger<JobsScheduler> _logger;
        private readonly SchedulesMethods _schedulesMethods;
        private readonly JobRunner _jobRunner;
        private readonly GroupsMethods _groupsMethods;

        public JobsScheduler(SchedulesMethods schedulesMethods, ILogger<JobsScheduler> logger, JobRunner jobRunner, GroupsMethods groupsMethods)
        {
            _schedulesMethods = schedulesMethods;
            _logger = logger;
            _jobRunner = jobRunner;
            _groupsMethods = groupsMethods;

            PopulateJobsQueue();

            //Updates the Jobs list every minute
            UpdateJobsTimer = new Timer
            {
                Interval = 60000
            };
            UpdateJobsTimer.Elapsed += (object sender, ElapsedEventArgs e) => PopulateJobsQueue();
            UpdateJobsTimer.Start();
        }

        /// <summary>
        /// Updates the queue
        /// </summary>
        private async void PopulateJobsQueue()
        {
            Jobs.Clear();

            foreach (Schedule schedule in await _schedulesMethods.GetSchedulesAsync())
                if (schedule.Job != null)
                    AddJob(schedule);

            UpdateWakeUpTimer();
        }

        /// <summary>
        /// Sets the timer when the JobRunner should "wake up" and run the scheduled job.
        /// </summary>
        private void UpdateWakeUpTimer()
        {
            if (WakeUpTimer == null)
            {
                WakeUpTimer = new Timer();
                WakeUpTimer.Elapsed += WakeUp;
            }

            if (Jobs.Count == 0)
                return;

            DateTime nextjob = Jobs.FirstOrDefault().Key;
            WakeUpTimer.Interval = (nextjob - DateTime.Now).TotalMilliseconds;
            WakeUpTimer.Start();
        }

        /// <summary>
        /// Runs the first job in the queue and updates the queue
        /// </summary>
        private async void WakeUp(object sender, ElapsedEventArgs e)
        {
            WakeUpTimer.Stop();

            //Run job
            var groups = await _groupsMethods.GetGroupsAsync();
            //var res = groups.SelectMany(x => x.GroupNodes.Where(y => y.GroupId == Jobs.FirstOrDefault().Value.Job.Group.Id));
            //TODO: Decide where to launch the job
            await _jobRunner.ExecuteAsync(Jobs.FirstOrDefault().Value.Job);

            RemoveExecutedJob();
            UpdateWakeUpTimer();
        }

        /// <summary>
        /// Removes the first job in the queue
        /// </summary>
        private void RemoveExecutedJob()
        {
            var job = Jobs.FirstOrDefault().Value;
            Jobs.Remove(Jobs.FirstOrDefault().Key);

            if (job == null)
                return;

            job.When = CrontabSchedule.Parse(job.Cron).GetNextOccurrence(DateTime.Now);
            Jobs.Add(job.When, job);
        }

        /// <summary>
        /// Adds a Job to the queued jobs
        /// </summary>
        /// <param name="schedule">The schedule to add</param>
        public void AddJob(Schedule schedule)
        {
            //Fail silently if no cron available
            if (string.IsNullOrEmpty(schedule.Cron))
                return;

            schedule.When = CrontabSchedule.Parse(schedule.Cron).GetNextOccurrence(DateTime.Now);
            Jobs.Add(schedule.When, schedule);
            _logger.LogInformation($"Added job {schedule.Id} - {schedule.Cron}");
        }

        /// <summary>
        /// Schedules a task to be run now (ASAP)
        /// </summary>
        /// <param name="schedule"></param>
        public void StartJobNow(Schedule schedule)
        {
            Jobs.Add(DateTime.Now, schedule);
            WakeUp(null, null);
            _logger.LogInformation($"Added job {schedule.Id}");
        }

        //TODO

        //public void RemoveJob()
        //{

        //}

        //public void EditJob()
        //{

        //}
    }
}
