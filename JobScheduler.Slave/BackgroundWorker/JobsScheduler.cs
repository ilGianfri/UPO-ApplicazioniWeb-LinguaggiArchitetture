using JobScheduler.Shared.Models;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobScheduler.Slave.BackgroundWorker
{

    public class JobsScheduler
    {
        public SortedList<DateTime, Schedule> Jobs = new SortedList<DateTime, Schedule>();
        private readonly ILogger<JobsScheduler> _logger;

        public JobsScheduler(ILogger<JobsScheduler> logger)
        {
            _logger = logger;


        }

        public void AddJob(Schedule schedule)
        {
            schedule.When = CrontabSchedule.Parse(schedule.Cron).GetNextOccurrence(DateTime.Now);
            Jobs.Add(schedule.When, schedule);
            _logger.LogInformation($"Added job {schedule.Id} - {schedule.Cron}");
        }

        public void RemoveExecutedJob()
        {
            var job = Jobs.FirstOrDefault().Value;
            Jobs.RemoveAt(0);

            job.When = CrontabSchedule.Parse(job.Cron).GetNextOccurrence(DateTime.Now);
            Jobs.Add(job.When, job);
        }

        public void RemoveJob()
        {

        }

        public void EditJob()
        {

        }
    }
}
