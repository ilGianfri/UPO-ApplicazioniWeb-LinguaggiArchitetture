using JobScheduler.Shared.Models;
using JobScheduler.Slave.BackgroundWorker;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JobScheduler.Slave.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        //private readonly JobsScheduler _jobsScheduler;
        //public JobsController(JobsScheduler jobsScheduler)
        //{
        //    _jobsScheduler = jobsScheduler;
        //}
        // GET: api/<JobsController>/running
        /// <summary>
        /// Returns the current running Job
        /// </summary>
        /// <returns></returns>
        [HttpGet("running")]
        public async Task<ActionResult<Job>> GetRunningJob()
        {
            //TODO
            return null;
        }

        // GET api/<JobsController>/status/5
        /// <summary>
        /// Returns the status of a Job given its id
        /// </summary>
        /// <param name="id">The Job id</param>
        /// <returns></returns>
        [HttpGet("status/{id}")]
        public async Task<ActionResult<JobStatus>> GetJobStatus(int id)
        {
            //TODO
            return null;
        }

        // POST api/<JobsController>/start
        /// <summary>
        /// Starts a new Job
        /// </summary>
        /// <param name="schedule">A Schedule object containing the Job to run</param>
        [HttpPost("start")]
        public async Task<ActionResult> StartJob([FromBody] Schedule schedule)
        {
            //_jobsScheduler.AddJob(DateTime.Now, schedule);

            return Ok();
        }

        /// <summary>
        /// Schedules a new job
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        [HttpPost("schedule")]
        public async Task<ActionResult> ScheduleJob([FromBody] Schedule schedule)
        {
            //_jobsScheduler.AddJob(DateTime.Now, schedule);

            return Ok();
        }
    }
}
