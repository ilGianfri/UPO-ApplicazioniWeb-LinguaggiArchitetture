using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers.API
{
    /// <summary>
    /// API userd by slaves to save a Job result
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class JobReportController : ControllerBase
    {
        private readonly JobReportMethods _jobReports;
        public JobReportController(JobReportMethods jobReports)
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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<JobResultController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<JobResultController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<JobResultController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
