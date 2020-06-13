using JobScheduler.Shared.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JobScheduler.Slave.BackgroundWorker
{
    public class JobRunner : BackgroundService
    {
        private readonly JobsScheduler _jobsScheduler;
        private DateTime? _nextRun;

        public JobRunner(JobsScheduler jobsScheduler)
        {
            _jobsScheduler = jobsScheduler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                if (_jobsScheduler.Jobs.Count > 0)
                {
                    Schedule nextSchedule = _jobsScheduler.Jobs.FirstOrDefault().Value;
                    _nextRun = nextSchedule?.When;
                    if (_nextRun != null)
                    {
                        if (DateTime.Now > _nextRun)
                        {
                            _jobsScheduler.RemoveExecutedJob();
                            if (nextSchedule.Job != null)
                            {
                                using Process jobProcess = new Process();
                                jobProcess.StartInfo.FileName = nextSchedule.Job.Path;
                                jobProcess.StartInfo.Arguments = nextSchedule.Job.Parameters;
                                jobProcess.StartInfo.UseShellExecute = false;
                                jobProcess.StartInfo.RedirectStandardOutput = true;
                                jobProcess.Exited += JobProcess_Exited;
                                jobProcess.Start();

                                //TODO: Send details to Master
                            }
                            await Task.Delay(5000, stoppingToken); //5 seconds delay
                        }
                    }
                }
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private void JobProcess_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            //TODO: Save result
            Console.WriteLine(p.StandardOutput.ReadToEnd());
        }
    }
}
