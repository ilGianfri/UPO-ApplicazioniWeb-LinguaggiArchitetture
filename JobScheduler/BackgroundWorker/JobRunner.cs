using JobScheduler.Controllers;
using JobScheduler.Data;
using JobScheduler.Shared.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace JobScheduler.BackgroundWorker
{
    public class JobRunner
    {
        private readonly JobReportMethods _jobReportMethods;
        //private readonly ApplicationDbContext _dbContext;

        public JobRunner(JobReportMethods jobReportMethods)
        {
            _jobReportMethods = jobReportMethods;
        }

        public void ExecuteAsync(Job job, CancellationToken cancellationToken)
        {
            //await Task.Run(async() =>
            //{
            if (job != null)
            {
                try
                {
                    Process jobProcess = new Process();
                    jobProcess.StartInfo.FileName = job.Path;
                    if (string.IsNullOrEmpty(job.Parameters))
                        jobProcess.StartInfo.Arguments = job.Parameters;
                    //jobProcess.StartInfo.UseShellExecute = false;
                    jobProcess.StartInfo.RedirectStandardOutput = true;

                    //If cancellation is requested, kills the process
                    cancellationToken.Register(() => jobProcess.Kill());

                    jobProcess.Exited += JobProcessExited;
                    var started = jobProcess.Start();
                    jobProcess.EnableRaisingEvents = true;
                    if (started)
                    {
                        //Save details
                        //await _jobReportMethods.CreateJobReportAsync(new JobReport() { JobId = job.Id, Pid = jobProcess.Id });

                    }
                }
                catch (Exception ex)
                {
                    //TODO: Save exception
                }

            }
            //}, cancellationToken);
        }

        private async void JobProcessExited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            //TODO: Save result
            //Console.WriteLine(p.StandardOutput.ReadToEnd());
            await _jobReportMethods.CreateJobReportAsync(new JobReport() { JobId = 0, Pid = p.Id, Output = p.StandardOutput.ReadToEnd(), ExitCode = p.ExitCode });
        }
    }
}
