using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers.API
{
    /// <summary>
    /// API userd by slaves to save a Job result
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobReportsController : ControllerBase
    {
        private readonly JobReportMethods _jobReports;
        public JobReportsController(JobReportMethods jobReports)
        {
            _jobReports = jobReports;
        }

        // GET: api/<JobResultController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobReport>>> Get()
        {
            return Ok(await _jobReports.GetJobReportsAsync());
        }

        // GET api/<JobResultController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobReport>> Get(int id)
        {
            return Ok(await _jobReports.GetJobReportAsync(id));
        }

        // POST api/<JobResultController>
        [HttpPost("create")]
        public async Task<ActionResult> Post([FromBody] JobReport value)
        {
            if (value == null)
                return BadRequest();

            try
            {
                await _jobReports.CreateJobReportAsync(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(201);
        }

        // PUT api/<JobResultController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JobReport>> Put(int id, [FromBody] JobReport value)
        {
            if (value == null)
                return BadRequest();

            JobReport job = await _jobReports.EditJobReportAsync(id, value);
            if (job != null)
                return Ok(job);

            return NotFound();
        }

        // DELETE api/<JobResultController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _jobReports.DeleteJobReportAsync(id);
        }
    }
}
