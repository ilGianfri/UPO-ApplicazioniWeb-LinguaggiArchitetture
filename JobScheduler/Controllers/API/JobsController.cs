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
    public class JobsController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        public JobsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<JobsController>
        [HttpGet]
        public ActionResult<IEnumerable<Job>> Get()
        {
            Job[] jobs = _dbContext.Jobs.ToArray();
            if (jobs == null)
                return new EmptyResult();

            return Ok(jobs);
        }

        // GET api/<JobsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(ulong id)
        {
            Job job = _dbContext.Jobs.FirstOrDefault(x => x.Id == id);
            if (job == null)
                return new EmptyResult();

            return Ok(job);
        }

        // POST api/<JobsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Job newJob)
        {
            if (newJob == null)
                return new EmptyResult();

            _dbContext.Jobs.Add(newJob);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<JobsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(ulong id, [FromBody] Job modifiedJob)
        {
            if (modifiedJob == null)
                return BadRequest();

            Job job = _dbContext.Jobs.FirstOrDefault(x => x.Id == id);
            if (job != null)
            {
                job = modifiedJob;

                await _dbContext.SaveChangesAsync();

                return Ok(job);
            }

            return NotFound();
        }

        // DELETE api/<JobsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(ulong id)
        {
            _dbContext.Jobs.Remove(_dbContext.Jobs.FirstOrDefault(x => x.Id == id));
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
