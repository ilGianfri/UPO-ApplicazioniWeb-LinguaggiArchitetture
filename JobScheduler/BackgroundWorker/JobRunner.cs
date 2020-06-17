using JobScheduler.Controllers;
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

        private int JobId;

        public JobRunner(JobReportMethods jobReportMethods)
        {
            _jobReportMethods = jobReportMethods;
        }

        public async Task ExecuteAsync(Job job, CancellationToken cancellationToken)
        {
            await Task.Run(async () =>
            {
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
                            //Save initial details & sets job as running
                            JobId = job.Id;
                            await _jobReportMethods.CreateJobReportAsync(new JobReport() { JobId = JobId, Pid = jobProcess.Id });
                             _jobReportMethods.SetJobStatus(JobId, JobStatus.Running);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: Save exception
                    }
                }
            }, cancellationToken);
        }

        private async void JobProcessExited(object sender, EventArgs e)
        {
            Process p = (Process)sender;

            //Save result & sets job as exited
            await _jobReportMethods.EditJobReportAsync(JobId, new JobReport() { JobId = JobId, Pid = p.Id, Output = p.StandardOutput.ReadToEnd(), ExitCode = p.ExitCode });
            _jobReportMethods.SetJobStatus(JobId, JobStatus.Exited);
        }
    }
}
