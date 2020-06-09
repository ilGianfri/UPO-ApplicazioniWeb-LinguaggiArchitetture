using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<IEnumerable<Node>> GetNodesAsync()
        {
            return await _dbContext.Nodes.ToListAsync();
        }

        public Node GetNodeByIdAsync(int id)
        {
            return _dbContext.Nodes.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> CreateNodeAsync(Node newNode)
        {
            _dbContext.Nodes.Add(newNode);
            var changes = await _dbContext.SaveChangesAsync();

            return changes > 0;
        }

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

        public async Task DeleteNodeAsync(int id)
        {
            _dbContext.Nodes.Remove(_dbContext.Nodes.FirstOrDefault(x => x.Id == id));
            await _dbContext.SaveChangesAsync();
        }
    }
}
