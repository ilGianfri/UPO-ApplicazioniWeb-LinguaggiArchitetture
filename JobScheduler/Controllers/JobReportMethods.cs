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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JobReport> GetJobReportAsync(int id)
        {
            JobReport report = await _dbContext.JobReports.FirstOrDefaultAsync(x => x.Id == id);

            return report;
        }
    }
}
