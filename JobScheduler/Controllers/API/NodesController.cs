using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JobScheduler.Data;
using JobScheduler.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NodesController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        public NodesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<NodesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node>>> Get()
        {
            List<Node> nodes = await _dbContext.Nodes.ToListAsync();
            return nodes == null ? new EmptyResult() : (ActionResult<IEnumerable<Node>>)Ok(nodes);
        }

        // GET api/<NodesController>/5
        [HttpGet("{id}")]
        public ActionResult<Node> Get(int id)
        {
            Node result = _dbContext.Nodes.FirstOrDefault(x => x.Id == id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<NodesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Node node)
        {
            if (node == null)
                return BadRequest();

            node.IP = IPAddress.Parse(node.IPStr);

            _dbContext.Nodes.Add(node);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<NodesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Node>> Put(int id, [FromBody] Node modifiedNode)
        {
            if (modifiedNode == null)
                return BadRequest();

            Node node = _dbContext.Nodes.FirstOrDefault(x => x.Id == id);
            if (node != null)
            {
                node.IPStr = modifiedNode.IPStr;
                node.Group = modifiedNode.Group;
                node.Name = modifiedNode.Name;
                node.Role = modifiedNode.Role;

                await _dbContext.SaveChangesAsync();

                return Ok(node);
            }

            return NotFound();
        }

        // DELETE api/<NodesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _dbContext.Nodes.Remove(_dbContext.Nodes.FirstOrDefault(x => x.Id == id));
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
