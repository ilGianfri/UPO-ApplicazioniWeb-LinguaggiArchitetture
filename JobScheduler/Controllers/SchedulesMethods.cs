using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class SchedulesMethods
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SchedulesMethods(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Returns all the schedules
        /// </summary>
        /// <returns>Returns a IEnumerable of Schedule objects</returns>
        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await db.Schedules.Include(Schedule => Schedule.Job).ToListAsync();
        }

        /// <summary>
        /// Gets a Schedule given its id
        /// </summary>
        /// <param name="id">The Schedule id</param>
        /// <returns>Returns a Schedule object</returns>
        public async Task<Schedule> GetScheduleByIdAsync(int id)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await db.Schedules.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new Schedule
        /// </summary>
        /// <param name="newSchedule">A Schedule object</param>
        /// <returns>Returns the newly created Schedule object otherwise null</returns>
        public async Task<Schedule> CreateScheduleAsync(Schedule newSchedule)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            db.Schedules.Add(newSchedule);
            int res = await db.SaveChangesAsync();

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
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            Schedule schedule = db.Schedules.FirstOrDefault(x => x.Id == id);
            if (schedule != null)
            {
                schedule.Cron = editedSchedule.Cron;
                schedule.JobId = editedSchedule.JobId;
                await db.SaveChangesAsync();

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
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            Schedule schedule = db.Schedules.FirstOrDefault(x => x.Id == id);
            if (schedule == null)
                return null;

            db.Schedules.Remove(schedule);
            int res = await db.SaveChangesAsync();

            return res > 0;
        }
    }
}
