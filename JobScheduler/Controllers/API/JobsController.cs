using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JobsController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private JobsMethods _jobMethods;
        public JobsController(ApplicationDbContext dbContext, JobsMethods jobMethods)
        {
            _dbContext = dbContext;
            _jobMethods = jobMethods;
        }

        // GET: api/<JobsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> Get()
        {
            return Ok((await _jobMethods.GetJobsAsync()).ToList());
        }

        // GET api/<JobsController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            Job job = _jobMethods.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();

            return Ok(job);
        }

        // POST api/<JobsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Job newJob)
        {
            if (newJob == null)
                return BadRequest();

            try
            {
                await _jobMethods.CreateJobAsync(newJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        // PUT api/<JobsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Job modifiedJob)
        {
            if (modifiedJob == null)
                return BadRequest();

            Job job = await _jobMethods.EditJobAsync(id, modifiedJob);
            if (job != null)
                return Ok(job);

            return NotFound();
        }

        // DELETE api/<JobsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _jobMethods.DeleteJobAsync(id);
            return Ok();
        }
    }
}
