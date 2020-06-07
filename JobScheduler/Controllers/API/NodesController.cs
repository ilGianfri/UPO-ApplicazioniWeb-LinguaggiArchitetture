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

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NodesController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private NodesMethods _nodesMethods;
        public NodesController(ApplicationDbContext dbContext, NodesMethods nodesMethods)
        {
            _dbContext = dbContext;
            _nodesMethods = nodesMethods;
        }

        // GET: api/<NodesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node>>> Get()
        {
            var nodes = await _nodesMethods.GetNodesAsync();
            return nodes == null ? new EmptyResult() : (ActionResult<IEnumerable<Node>>)Ok(nodes);
        }

        // GET api/<NodesController>/5
        [HttpGet("{id}")]
        public ActionResult<Node> Get(int id)
        {
            Node result = _nodesMethods.GetNodeByIdAsync(id);
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

            bool created = await _nodesMethods.CreateNodeAsync(node);

            return created ? StatusCode(201) : StatusCode(500);
        }

        // PUT api/<NodesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Node>> Put(int id, [FromBody] Node modifiedNode)
        {
            if (modifiedNode == null)
                return BadRequest();

            Node node = await _nodesMethods.EditNodeAsync(id, modifiedNode);
            if (node != null)
                return Ok(node);

            return NotFound();
        }

        // DELETE api/<NodesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _nodesMethods.DeleteNodeAsync(id);

            return Ok();
        }
    }
}
