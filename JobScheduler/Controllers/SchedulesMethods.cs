using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class SchedulesMethods
    {
        private readonly ApplicationDbContext _dbContext;
        public SchedulesMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all the schedules
        /// </summary>
        /// <returns>Returns a IEnumerable of Schedule objects</returns>
        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        {
            return await _dbContext.Schedules.Include(Schedule => Schedule.Job).ToListAsync();
        }

        /// <summary>
        /// Gets a Schedule given its id
        /// </summary>
        /// <param name="id">The Schedule id</param>
        /// <returns>Returns a Schedule object</returns>
        public async Task<Schedule> GetScheduleByIdAsync(int id)
        {
            return await _dbContext.Schedules.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new Schedule
        /// </summary>
        /// <param name="newSchedule">A Schedule object</param>
        /// <returns>Returns the newly created Schedule object otherwise null</returns>
        public async Task<Schedule> CreateScheduleAsync(Schedule newSchedule)
        {
            _dbContext.Schedules.Add(newSchedule);
            var res = await _dbContext.SaveChangesAsync();

            if (res > 0)
                return newSchedule;

            return null;
        }

        /// <summary>
        /// Edits a specific node
        /// </summary>
        /// <param name="id">The Schedule id</param>
        /// <param name="editedSchedule">The modified Schedule object</param>
        /// <returns>Returns the modified Schedule object if successful</returns>
        /// <returns></returns>
        public async Task<Schedule> EditScheduleAsync(int id, Schedule editedSchedule)
        {
            Schedule schedule = _dbContext.Schedules.FirstOrDefault(x => x.Id == id);
            if (schedule != null)
            {
                schedule = editedSchedule;
                await _dbContext.SaveChangesAsync();

                return schedule;
            }

            return null;
        }

        /// <summary>
        /// Deletes the Schedule identified by the gived id
        /// </summary>
        /// <param name="id">The Schedule id</param>
        /// <returns>Returns a boolean with the operation result</returns>
        public async Task<bool?> DeleteScheduleAsync(int id)
        {
            Schedule schedule = _dbContext.Schedules.FirstOrDefault(x => x.Id == id);
            if (schedule == null)
                return null;

            _dbContext.Schedules.Remove(schedule);
            var res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }
    }
}
