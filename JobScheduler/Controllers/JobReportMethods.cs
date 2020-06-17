using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class JobReportMethods
    {
        //private readonly ApplicationDbContext _dbContext;
        private IServiceScopeFactory _serviceScopeFactory;

        public JobReportMethods(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Returns all the Job Reports from the database
        /// </summary>
        /// <returns>Returns a IEnumerable of JobReport objects</returns>
        public async Task<IEnumerable<JobReport>> GetJobReportsAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            return await db.JobReports.ToListAsync();
        }

        /// <summary>
        /// Gets the JobReport with the specified id
        /// </summary>
        /// <param name="id">The id of the Job</param>
        /// <returns></returns>
        public async Task<JobReport> GetJobReportAsync(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            JobReport report = await db.JobReports.FirstOrDefaultAsync(x => x.Id == id);

            return report;
        }

        /// <summary>
        /// Sets a Job status
        /// </summary>
        /// <param name="jobId">The id of the Job</param>
        /// <param name="jobStatus">New status to set</param>
        public async void SetJobStatus(int jobId, JobStatus jobStatus)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var job = await db.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
            if (job == null)
                return;

            job.Status = jobStatus;

            await db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The id of the Job</param>
        /// <param name="newReport">The edited JobReport</param>
        /// <returns></returns>
        public async Task<JobReport> EditJobReportAsync(int id, JobReport newReport)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var report = await db.JobReports.FirstOrDefaultAsync(x => x.Id == id);

            if (report != null)
            {
                report = newReport;
                var res = await db.SaveChangesAsync();
            }
            return report;
        }

        /// <summary>
        /// Creates a new JobReport
        /// </summary>
        /// <returns>The job report id</returns>
        public async Task<int?> CreateJobReportAsync(JobReport report)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (report == null)
                return null;

            var res = db.JobReports.Add(report);

            await db.SaveChangesAsync();

            return res.Entity.Id;
        }

        /// <summary>
        /// Deletes the Job Report with the given id
        /// </summary>
        /// <param name="jobId">The JobReport id to delete</param>
        public async void DeleteJobReportAsync(int jobId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var report = await db.JobReports.FirstOrDefaultAsync(x => x.Id == jobId);
            if (report == null)
                return;

            db.JobReports.Remove(report);

            await db.SaveChangesAsync();
        }
    }
}
