using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobScheduler.Data;
using JobScheduler.Models;
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
        private ApplicationDbContext _dbContext;
        public NodesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<NodesController>
        [HttpGet]
        public ActionResult<IEnumerable<Node>> Get()
        {
            return Ok(_dbContext.Nodes.ToArray());
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

            _dbContext.Nodes.Add(node);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<NodesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NodesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
