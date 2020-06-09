﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NodesController : ControllerBase
    {
        private NodesMethods _nodesMethods;
        public NodesController(NodesMethods nodesMethods)
        {
            _nodesMethods = nodesMethods;
        }

        // GET: api/<NodesController>
        /// <summary>
        /// Returns all the nodes
        /// </summary>
        /// <returns>Returns a IEnumerable of Node objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node>>> Get()
        {
            var nodes = await _nodesMethods.GetNodesAsync();
            return nodes == null ? new EmptyResult() : (ActionResult<IEnumerable<Node>>)Ok(nodes);
        }

        // GET api/<NodesController>/5
        /// <summary>
        /// Returns a specific Node given its id
        /// </summary>
        /// <param name="id">The id of the Node to return</param>
        /// <returns>Returns a Node object</returns>
        [HttpGet("{id}")]
        public ActionResult<Node> Get(int id)
        {
            Node result = _nodesMethods.GetNodeByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<NodesController>
        /// <summary>
        /// Creates a new Node
        /// </summary>
        /// <param name="node">The Node object to create</param>
        /// <returns>Returns 201 if created successfully otherwise 400</returns>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Node node)
        {
            if (node == null)
                return BadRequest();

            node.IP = IPAddress.Parse(node.IPStr);

            bool created = await _nodesMethods.CreateNodeAsync(node);

            return created ? StatusCode(201) : StatusCode(400);
        }

        // PUT api/<NodesController>/5
        /// <summary>
        /// Edits an existing node
        /// </summary>
        /// <param name="id">The id of the Node to edit</param>
        /// <param name="modifiedNode">The modified Node object</param>
        /// <returns>The modified Node object if successful</returns>
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
        /// <summary>
        /// Deletes the Node with the specific id
        /// </summary>
        /// <param name="id">The Node id</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _nodesMethods.DeleteNodeAsync(id);

            return Ok();
        }
    }
}
