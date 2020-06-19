using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers
{
    public class NodesMethods
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public NodesMethods(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Returns all the nodes
        /// </summary>
        /// <returns>Returns a IEnumerable of Node objects</returns>
        public async Task<IEnumerable<Node>> GetNodesAsync()
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await db.Nodes.Include(x => x.GroupNodes).ThenInclude(g => g.Group).ToListAsync();
        }

        /// <summary>
        /// Returns a specific Node given its id
        /// </summary>
        /// <param name="id">The id of the Node to return</param>
        /// <returns>Returns a Node object</returns>
        public async Task<Node> GetNodeByIdAsync(int id)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await db.Nodes.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new Node
        /// </summary>
        /// <param name="newNode">The Node object to create</param>
        /// <returns>Returns true if created successfully</returns>
        public async Task<Node> CreateNodeAsync(Node newNode)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Node> node = db.Nodes.Add(newNode);
            int changes = await db.SaveChangesAsync();

            return changes > 0 ? node.Entity : null;
        }

        /// <summary>
        /// Edits an existing node
        /// </summary>
        /// <param name="id">The id of the Node to edit</param>
        /// <param name="editedNode">The modified Node object</param>
        /// <returns>The modified Node object if successful otherwise null</returns>
        public async Task<Node> EditNodeAsync(int id, Node editedNode)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            Node node = db.Nodes.FirstOrDefault(x => x.Id == id);
            if (node != null)
            {
                node = editedNode;

                await db.SaveChangesAsync();
            }

            return node;
        }

        /// <summary>
        /// Deletes the Node with the specific id
        /// </summary>
        /// <param name="id">The Node id</param>
        public async Task DeleteNodeAsync(int id)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            ApplicationDbContext db = scope.ServiceProvider.GetService<ApplicationDbContext>();

            //Removes the relations first
            IQueryable<GroupNode> nodes = db.GroupNodes.Where(x => x.NodeId == id);

            foreach (GroupNode n in nodes)
                db.GroupNodes.Remove(n);

            await db.SaveChangesAsync();

            //Deletes the node
            db.Nodes.Remove(db.Nodes.FirstOrDefault(x => x.Id == id));
            await db.SaveChangesAsync();
        }
    }
}
