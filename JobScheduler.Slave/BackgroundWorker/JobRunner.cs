using JobScheduler.Shared.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
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
                        bool started = jobProcess.Start();
                        jobProcess.EnableRaisingEvents = true;
                        if (started)
                        {
                            JobId = job.Id;
                            using HttpClient client = new HttpClient();
                            StringContent content = new StringContent(JsonSerializer.Serialize(new JobReport() { JobId = job.Id, Pid = jobProcess.Id, StartTime = jobProcess.StartTime }), Encoding.UTF8, "application/json");
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.JWToken);
                            HttpResponseMessage httpResponse = await client.PostAsync($"{Constants.ServerUrl}/api/JobReports/create", content);
                            if (httpResponse.IsSuccessStatusCode)
                                ReportId = Convert.ToInt32(await httpResponse.Content.ReadAsStringAsync());
                        }
                    }
                    catch
                    {

                    }

                }
            });
        }

        private async void JobProcessExited(object sender, EventArgs e)
        {
            try
            {
                Process p = (Process)sender;
                using HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonSerializer.Serialize(new JobReport() { Id = ReportId, JobId = JobId, Pid = p.Id, Output = p.StandardOutput.ReadToEnd(), ExitCode = p.ExitCode, ExitTime = p.ExitTime }), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constants.JWToken); 
                HttpResponseMessage httpResponse = await client.PutAsync($"{Constants.ServerUrl}/api/JobReports/update/{ReportId}", content);
            }
            catch
            {

            }
        }
    }
}
