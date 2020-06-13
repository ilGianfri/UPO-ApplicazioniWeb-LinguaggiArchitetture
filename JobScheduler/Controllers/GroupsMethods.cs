using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> DeleteGroupAsync(int id)
        {
            _dbContext.Groups.Remove(_dbContext.Groups.FirstOrDefault(x => x.Id == id));
            var res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<bool> CreateGroupAsync(Group newGroup)
        {
            _dbContext.Groups.Add(newGroup);
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        public async Task<Group> EditGroupAsync(int id, Group editedGroup)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == id);

            if (group != null)
            {
                group = editedGroup;
                var res = await _dbContext.SaveChangesAsync();
                if (res > 0)
                    return group;
            }
            return group;
        }
    }
}
