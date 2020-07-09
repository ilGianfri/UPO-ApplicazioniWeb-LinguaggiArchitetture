using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class JobsMethods
    {
        private readonly ApplicationDbContext _dbContext;
        public JobsMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all the jobs from the database
        /// </summary>
        public async Task<IEnumerable<Job>> GetJobsAsync() => await _dbContext.Jobs.Include(x => x.Group).ToListAsync();

        /// <summary>
        /// Returns the Job with the specified id
        /// </summary>
        /// <param name="id">The id of the job</param>
        public async Task<Job> GetJobByIdAsync(int id) => await _dbContext.Jobs.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Creates a new job
        /// </summary>
        /// <param name="newJob">The job object to create in the database</param>
        public async Task<bool> CreateJobAsync(Job newJob)
        {
            _dbContext.Jobs.Add(newJob);
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        /// <summary>
        /// Edits the Job with the specified id
        /// </summary>
        /// <param name="id">The id of the job to edit</param>
        /// <param name="editedJob">The new job content</param>
        public async Task<Job> EditJobAsync(int id, Job editedJob)
        {
            Job job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if (job != null)
            {
                job = editedJob;

                int res = await _dbContext.SaveChangesAsync();
                if (res > 0)
                    return job;
            }

            return null;
        }

        /// <summary>
        /// Kills jobs with the specified id
        /// </summary>
        /// <param name="jobId">Job id</param>
        /// <returns></returns>
        public async Task<bool> KillRunningJobById(int jobId)
        {
            //Gets all jobs that haven't completed with given id
            List<JobReport> runningJobs = await _dbContext.JobReports.Where(x => x.ExitTime == null && x.JobId == jobId).ToListAsync();
            if (runningJobs != null)
            {
                foreach (JobReport job in runningJobs)
                {
                    try
                    {
                        if (job.Pid.HasValue)
                            Process.GetProcessById(job.Pid.Value).Kill();
                    }
                    catch { }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Kills the job with the specified pid
        /// </summary>
        /// <param name="jobPid">Job Pid/param>
        /// <returns></returns>
        public bool KillRunningJobByPid(int jobPid)
        {
            try
            {
                Process.GetProcessById(jobPid).Kill();
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// Deletes the Job with the specified id
        /// </summary>
        /// <param name="id">The id of the job to delete</param>
        /// <returns>Returns true if successful</returns>
        public async Task<bool> DeleteJobAsync(int id)
        {
            _dbContext.Jobs.Remove(await _dbContext.Jobs.FirstOrDefaultAsync(x => x.Id == id));
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }
    }
}
