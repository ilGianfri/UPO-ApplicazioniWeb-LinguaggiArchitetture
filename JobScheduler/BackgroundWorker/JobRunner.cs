using JobScheduler.Controllers;
using JobScheduler.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobScheduler.BackgroundWorker
{
    public class JobRunner
    {
        private readonly JobReportMethods _jobReportMethods;
        private readonly GroupsMethods _groupsMethods;
        private readonly NodesMethods _nodesMethods;

        private int JobId;
        private int? ReportId;

        public JobRunner(JobReportMethods jobReportMethods, GroupsMethods groupsMethods, NodesMethods nodesMethods)
        {
            _jobReportMethods = jobReportMethods;
            _groupsMethods = groupsMethods;
        }

        /// <summary>
        /// Runs a job on a specific group - used when the user clicks on "run now"
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <param name="job">The job object</param>
        /// <returns></returns>
        public async Task RunJobOnGroup(int? groupId, Job job)
        {
            await Task.Run(async () =>
            {
                if (job != null)
                {
                    //Get available groups
                    IEnumerable<Group> groups = await _groupsMethods.GetGroupsAsync();

                    if (groupId == null) //Run job on all nodes
                    {
                        //Run job locally
                        await ExecuteAsync(job);

                        foreach (Node node in await _nodesMethods.GetNodesAsync())
                        {
                            try
                            {
                                using HttpClient client = new HttpClient();
                                StringContent content = new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, "application/json");
                                await client.PostAsync($"{node.IPStr}:{node.Port}/api/jobs/start", content);
                            }
                            catch { }
                        }
                    }
                    else //Run job on defined group
                    {
                        IEnumerable<Node> nodes = groups.FirstOrDefault(x => x.Id == groupId).GroupNodes.Select(x => x.Node);
                        foreach (Node node in nodes)
                        {
                            if (node.Role == NodeRole.Master)
                            {
                                //Run job locally
                                await ExecuteAsync(job);
                            }
                            else
                            {
                                try
                                {
                                    using HttpClient client = new HttpClient();
                                    StringContent content = new StringContent(JsonSerializer.Serialize(job), Encoding.UTF8, "application/json");
                                    var res = await client.PostAsync($"{node.IPStr}:{node.Port}/api/jobs/start", content);
                                }
                                catch (Exception e) { }
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Runs the given job locally
        /// </summary>
        /// <param name="job">The job to run</param>
        /// <returns></returns>
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
                            //Save initial details & sets job as running
                            JobId = job.Id;
                            ReportId = await _jobReportMethods.CreateJobReportAsync(new JobReport() { JobId = JobId, Pid = jobProcess.Id, StartTime = jobProcess.StartTime });
                            _jobReportMethods.SetJobStatus(JobId, JobStatus.Running);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        /// <summary>
        /// Saves the job reports with all the details of the finished job
        /// </summary>
        private async void JobProcessExited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            //Save result & sets job as exited
            await _jobReportMethods.EditJobReportAsync(ReportId.Value, new JobReport() { Id = ReportId.Value, JobId = JobId, Pid = p.Id, Output = p.StandardOutput.ReadToEnd(), ExitCode = p.ExitCode, ExitTime = p.ExitTime });
            _jobReportMethods.SetJobStatus(JobId, JobStatus.Exited);
        }
    }
}
