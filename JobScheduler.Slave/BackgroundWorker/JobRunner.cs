using JobScheduler.Shared.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobScheduler.Slave.BackgroundWorker
{
    public class JobRunner
    {
        private int ReportId, JobId;
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
                            JobId = job.Id;
                            //TODO: Remove hardcoded values 
                            using HttpClient client = new HttpClient();
                            StringContent content = new StringContent(JsonSerializer.Serialize(new JobReport() { JobId = job.Id, Pid = jobProcess.Id, StartTime = jobProcess.StartTime }), Encoding.UTF8, "application/json");
                            var httpResponse = await client.PostAsync("https://localhost:44383/api/JobReports/create", content);
                            if (httpResponse.IsSuccessStatusCode)
                                ReportId = Convert.ToInt32(httpResponse.Content.ReadAsStringAsync());
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
            try
            {
                Process p = (Process)sender;
                //TODO: Remove hardcoded values 
                using HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonSerializer.Serialize(new JobReport() { Id = ReportId, JobId = JobId, Pid = p.Id, Output = p.StandardOutput.ReadToEnd(), ExitCode = p.ExitCode, ExitTime = p.ExitTime }), Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync($"https://localhost:44383/api/JobReports/update/{ReportId}", content);
            }
            catch
            {

            }
        }
    }
}
