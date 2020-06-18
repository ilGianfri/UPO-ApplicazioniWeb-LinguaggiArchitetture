using JobScheduler.Controllers;
using JobScheduler.Shared.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JobScheduler.BackgroundWorker
{
    public class JobRunner
    {
        private readonly JobReportMethods _jobReportMethods;

        private int JobId;
        private int? ReportId;

        public JobRunner(JobReportMethods jobReportMethods)
        {
            _jobReportMethods = jobReportMethods;
        }

        public async Task ExecuteAsync(Job job)
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
                        jobProcess.StartInfo.UseShellExecute = false;
                        jobProcess.StartInfo.RedirectStandardOutput = true;

                        jobProcess.Exited += JobProcessExited;
                        var started = jobProcess.Start();
                        jobProcess.EnableRaisingEvents = true;
                        if (started)
                        {
                            //Save initial details & sets job as running
                            JobId = job.Id;
                            ReportId = await _jobReportMethods.CreateJobReportAsync(new JobReport() { JobId = JobId, Pid = jobProcess.Id, StartTime = jobProcess.StartTime });
                            _jobReportMethods.SetJobStatus(JobId, JobStatus.Running);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: Save exception
                    }
                }
            });
        }

        private async void JobProcessExited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            //Save result & sets job as exited
            await _jobReportMethods.EditJobReportAsync(ReportId.Value, new JobReport() { Id = ReportId.Value, JobId = JobId, Pid = p.Id, Output = p.StandardOutput.ReadToEnd(), ExitCode = p.ExitCode, ExitTime = p.ExitTime });
            _jobReportMethods.SetJobStatus(JobId, JobStatus.Exited);
        }
    }
}
