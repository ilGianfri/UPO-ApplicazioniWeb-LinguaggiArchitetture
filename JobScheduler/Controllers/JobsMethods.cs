using JobScheduler.Data;
using JobScheduler.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class JobsMethods
    {
        private ApplicationDbContext _dbContext;
        public JobsMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all the jobs from the database
        /// </summary>
        public async Task<IEnumerable<Job>> GetJobsAsync()
        {
            return await _dbContext.Jobs.ToListAsync();
        }

        /// <summary>
        /// Returns the Job with the specified id
        /// </summary>
        /// <param name="id">The id of the job</param>
        public Job GetJobByIdAsync(int id)
        {
            return _dbContext.Jobs.FirstOrDefault(x => x.Id == id);
        }

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
            Job job = _dbContext.Jobs.FirstOrDefault(x => x.Id == id);
            if (job != null)
            {
                job = editedJob;

                var res = await _dbContext.SaveChangesAsync();
                if (res > 0)
                    return job;
            }

            return null;
        }

        /// <summary>
        /// Deletes the Job with the specified id
        /// </summary>
        /// <param name="id">The id of the job to delete</param>
        /// <returns>Returns true if successful</returns>
        public async Task<bool> DeleteJobAsync(int id)
        {
            _dbContext.Jobs.Remove(_dbContext.Jobs.FirstOrDefault(x => x.Id == id));
            var res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }
    }
}
