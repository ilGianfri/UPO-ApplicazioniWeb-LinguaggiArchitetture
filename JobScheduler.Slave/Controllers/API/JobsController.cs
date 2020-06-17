using JobScheduler.Shared.Models;
using JobScheduler.Slave.BackgroundWorker;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace JobScheduler.Slave.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly JobRunner _jobRunner;
        public JobsController(JobRunner jobRunner)
        {
            _jobRunner = jobRunner;
        }

        // GET api/<JobsController>/cancel/5
        /// <summary>
        /// Returns the status of a Job given its id
        /// </summary>
        /// <param name="id">The Job PID</param>
        /// <returns></returns>
        [HttpPost("kill/{id}")]
        public ActionResult CancelJob(int id)
        {
            try
            {
                Process.GetProcessById(id).Kill();

            }
            catch
            {
                return NotFound();
            }

            //Accepted
            return StatusCode(202);
        }

        // POST api/<JobsController>/start
        /// <summary>
        /// Starts a new Job
        /// </summary>
        /// <param name="job">A Schedule object containing the Job to run</param>
        [HttpPost("start")]
        public async Task<ActionResult> StartJob([FromBody] Job job)
        {
            await _jobRunner.ExecuteAsync(job);

            //Accepted
            return StatusCode(202);
        }
    }
}
