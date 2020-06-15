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

        /// <summary>
        /// Geta all the Groups
        /// </summary>
        /// <returns>Returns a IEnumberable of Group objects</returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await _dbContext.Groups.ToListAsync();
        }

        /// <summary>
        /// Gets the Group with the id specified
        /// </summary>
        /// <param name="id">The Group id</param>
        /// <returns>Returns a Group object</returns>
        public async Task<Group> GetGroupByIdAsync(int id)
        {
            return await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Deletes the Group with the specified id
        /// </summary>
        /// <param name="id">The Group id</param>
        /// <returns>Returns true if deleted successfully otherwise false</returns>
        public async Task<bool> DeleteGroupAsync(int id)
        {
            _dbContext.Groups.Remove(_dbContext.Groups.FirstOrDefault(x => x.Id == id));
            var res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        /// <summary>
        /// Creates a new group
        /// </summary>
        /// <param name="newGroup">The Group to add to the database</param>
        /// <returns>Returns true if created successfully otherwise false</returns>
        public async Task<bool> CreateGroupAsync(Group newGroup)
        {
            _dbContext.Groups.Add(newGroup);
            int res = await _dbContext.SaveChangesAsync();

            return res > 0;
        }

        /// <summary>
        /// Edits the specified node
        /// </summary>
        /// <param name="id">The Group id</param>
        /// <param name="editedGroup">The modified group</param>
        /// <returns>Returns the modified group. If the group with the given id does not exist, returns null</returns>
        public async Task<Group> EditGroupAsync(int id, Group editedGroup)
        {
            var group = await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == id);

            if (group != null)
            {
                group = editedGroup;
                var res = await _dbContext.SaveChangesAsync();
            }
            return group;
        }
    }
}
