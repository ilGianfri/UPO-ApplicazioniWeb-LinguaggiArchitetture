using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers.API
{
    /// <summary>
    /// CRUD API for Jobs
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JobsController : ControllerBase
    {
        private readonly JobsMethods _jobMethods;
        public JobsController(JobsMethods jobMethods)
        {
            _jobMethods = jobMethods;
        }

        // GET: api/<JobsController>
        /// <summary>
        /// Gets all the Jobs
        /// </summary>
        /// <returns>Returns a IEnumerable of Jobs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> Get()
        {
            return Ok((await _jobMethods.GetJobsAsync()).ToList());
        }

        // GET api/<JobsController>/5
        /// <summary>
        /// Gets a specific Job identified by its id
        /// </summary>
        /// <param name="id">The job id</param>
        /// <returns>Returns a Job object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetAsync(int id)
        {
            Job job = await _jobMethods.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();

            return Ok(job);
        }

        // POST api/<JobsController>
        /// <summary>
        /// Creates a new job
        /// </summary>
        /// <param name="newJob">A Job object</param>
        /// <returns>Returns 201 if succesful, otherwise 400.</returns>
        [HttpPost("create")]
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

            return StatusCode(201);
        }

        // PUT api/<JobsController>/5
        /// <summary>
        /// Edits an existing Job identified by its id
        /// </summary>
        /// <param name="id">The id of the Job to edit</param>
        /// <param name="modifiedJob">The modified Job object</param>
        /// <returns></returns>
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
        /// <summary>
        /// Deletes a specific job identified by its id
        /// </summary>
        /// <param name="id">The job id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _jobMethods.DeleteJobAsync(id);
            return Ok();
        }
    }
}
