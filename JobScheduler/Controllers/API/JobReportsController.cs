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

        // GET: api/<JobReportsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobReport>>> Get() => Ok(await _jobReports.GetJobReportsAsync());

        // GET api/<JobReportsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobReport>> Get(int id) => Ok(await _jobReports.GetJobReportAsync(id));

        // POST api/<JobReportsController>/create
        [HttpPost("create")]
        public async Task<ActionResult<int>> Post([FromBody] JobReport value)
        {
            if (value == null)
                return BadRequest();

            try
            {
                int? reportId = await _jobReports.CreateJobReportAsync(value);

                return Ok(reportId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<JobReportsController>/update/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<JobReport>> Put(int id, [FromBody] JobReport value)
        {
            if (value == null)
                return BadRequest();

            JobReport job = await _jobReports.EditJobReportAsync(id, value);
            return job != null ? Ok(job) : (ActionResult<JobReport>)NotFound();
        }

        // DELETE api/<JobReportsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) => _jobReports.DeleteJobReportAsync(id);
    }
}
