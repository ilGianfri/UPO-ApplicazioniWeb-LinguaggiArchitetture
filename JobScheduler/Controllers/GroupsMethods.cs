using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class GroupsMethods
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public GroupsMethods(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Geta all the Groups
        /// </summary>
        /// <returns>Returns a IEnumberable of Group objects</returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await db.Groups.Include(x => x.GroupNodes).ToListAsync();
        }

        /// <summary>
        /// Gets the Group with the id specified
        /// </summary>
        /// <param name="id">The Group id</param>
        /// <returns>Returns a Group object</returns>
        public async Task<Group> GetGroupByIdAsync(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await db.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Deletes the Group with the specified id
        /// </summary>
        /// <param name="id">The Group id</param>
        /// <returns>Returns true if deleted successfully otherwise false</returns>
        public async Task<bool> DeleteGroupAsync(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            db.Groups.Remove(await db.Groups.FirstOrDefaultAsync(x => x.Id == id));
            var res = await db.SaveChangesAsync();

            return res > 0;
        }

        /// <summary>
        /// Creates a new group
        /// </summary>
        /// <param name="newGroup">The Group to add to the database</param>
        /// <returns>Returns true if created successfully otherwise false</returns>
        public async Task<bool> CreateGroupAsync(Group newGroup)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            db.Groups.Add(newGroup);
            int res = await db.SaveChangesAsync();

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
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var group = await db.Groups.FirstOrDefaultAsync(x => x.Id == id);

            if (group != null)
            {
                group = editedGroup;
                var res = await db.SaveChangesAsync();
            }
            return group;
        }
    }
}
