using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace JobScheduler.Controllers
{
    public class NodesMethods
    {
        private ApplicationDbContext _dbContext;
        public NodesMethods(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all the nodes
        /// </summary>
        /// <returns>Returns a IEnumerable of Node objects</returns>
        public async Task<IEnumerable<Node>> GetNodesAsync()
        {
            return await _dbContext.Nodes.ToListAsync();
        }

        /// <summary>
        /// Returns a specific Node given its id
        /// </summary>
        /// <param name="id">The id of the Node to return</param>
        /// <returns>Returns a Node object</returns>
        public Node GetNodeByIdAsync(int id)
        {
            return _dbContext.Nodes.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Creates a new Node
        /// </summary>
        /// <param name="newNode">The Node object to create</param>
        /// <returns>Returns true if created successfully</returns>
        public async Task<bool> CreateNodeAsync(Node newNode)
        {
            _dbContext.Nodes.Add(newNode);
            var changes = await _dbContext.SaveChangesAsync();

            return changes > 0;
        }

        /// <summary>
        /// Edits an existing node
        /// </summary>
        /// <param name="id">The id of the Node to edit</param>
        /// <param name="editedNode">The modified Node object</param>
        /// <returns>The modified Node object if successful otherwise null</returns>
        public async Task<Node?> EditNodeAsync(int id, Node editedNode)
        {
            Node node = _dbContext.Nodes.FirstOrDefault(x => x.Id == id);
            if (node != null)
            {
                node.IPStr = editedNode.IPStr;
                node.Group = editedNode.Group;
                node.Name = editedNode.Name;
                node.Role = editedNode.Role;

                await _dbContext.SaveChangesAsync();
            }

            return node;
        }

        /// <summary>
        /// Deletes the Node with the specific id
        /// </summary>
        /// <param name="id">The Node id</param>
        public async Task DeleteNodeAsync(int id)
        {
            _dbContext.Nodes.Remove(_dbContext.Nodes.FirstOrDefault(x => x.Id == id));
            await _dbContext.SaveChangesAsync();
        }
    }
}
