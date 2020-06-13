using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class GroupsMethods
    {
        private readonly ApplicationDbContext _dbContext;
        public GroupsMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await _dbContext.Groups.ToListAsync();
        }
    }
}
