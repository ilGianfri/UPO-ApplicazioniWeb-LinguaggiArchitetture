using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class SchedulesMethods
    {
        private ApplicationDbContext _dbContext;
        public SchedulesMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all the schedules
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Schedule>> GetSchedulesAsync()
        {
            return await _dbContext.Schedules.ToListAsync();
        }

        public Schedule GetScheduleByIdAsync(int id)
        {
            return _dbContext.Schedules.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Schedule> CreateScheduleAsync(Schedule newSchedule)
        {
            _dbContext.Schedules.Add(newSchedule);
            var res = await _dbContext.SaveChangesAsync();

            if (res > 0)
                return newSchedule;

            return null;
        }

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
