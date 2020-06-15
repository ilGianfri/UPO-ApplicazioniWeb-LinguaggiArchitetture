using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class JobReportMethods
    {
        private readonly ApplicationDbContext _dbContext;
        public JobReportMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all the Job Reports from the database
        /// </summary>
        /// <returns>Returns a IEnumerable of JobReport objects</returns>
        public async Task<IEnumerable<JobReport>> GetJobReportsAsync()
        {
            return await _dbContext.JobReports.ToListAsync();
        }

        /// <summary>
        /// Gets the JobReport with the specified id
        /// </summary>
        /// <param name="id">The id of the Job</param>
        /// <returns></returns>
        public async Task<JobReport> GetJobReportAsync(int id)
        {
            JobReport report = await _dbContext.JobReports.FirstOrDefaultAsync(x => x.Id == id);

            return report;
        }

        /// <summary>
        /// Sets a Job status
        /// </summary>
        /// <param name="jobId">The id of the Job</param>
        /// <param name="jobStatus">New status to set</param>
        public async void SetJobStatus(int jobId, JobStatus jobStatus)
        {
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
            if (job == null)
                return;

            job.Status = jobStatus;

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The id of the Job</param>
        /// <param name="newReport">The edited JobReport</param>
        /// <returns></returns>
        public async Task<JobReport> EditJobReportAsync(int id, JobReport newReport)
        {
            var report = await _dbContext.JobReports.FirstOrDefaultAsync(x => x.Id == id);

            if (report != null)
            {
                report = newReport;
                var res = await _dbContext.SaveChangesAsync();
            }
            return report;
        }

        /// <summary>
        /// Creates a new JobReport
        /// </summary>
        /// <returns>A boolean representing the success of the operation</returns>
        public async Task<bool> CreateJobReportAsync(JobReport report)
        {
            if (report == null)
                return false;

            _dbContext.JobReports.Add(report);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes the Job Report with the given id
        /// </summary>
        /// <param name="jobId">The JobReport id to delete</param>
        public async void DeleteJobReportAsync(int jobId)
        {
            var report = await _dbContext.JobReports.FirstOrDefaultAsync(x => x.Id == jobId);
            if (report == null)
                return;

            _dbContext.JobReports.Remove(report);

            await _dbContext.SaveChangesAsync();
        }
    }
}
