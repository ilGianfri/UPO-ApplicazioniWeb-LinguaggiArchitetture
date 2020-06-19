using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers.API
{
    /// <summary>
    /// CRUD API for Schedules
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SchedulesController : ControllerBase
    {
        private readonly SchedulesMethods _schedulesMethods;
        public SchedulesController(SchedulesMethods schedulesMethods)
        {
            _schedulesMethods = schedulesMethods;
        }

        // GET: api/<SchedulesController>
        /// <summary>
        /// Returns all the schedules
        /// </summary>
        /// <returns>Returns a IEnumerable of Schedule objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> Get()
        {
            List<Schedule> schedules = (await _schedulesMethods.GetSchedulesAsync()).ToList();

            return Ok(schedules);
        }

        // GET api/<SchedulesController>/5
        /// <summary>
        /// Gets a Schedule given its id
        /// </summary>
        /// <param name="id">The Schedule id</param>
        /// <returns>Returns a Schedule object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetAsync(int id)
        {
            Schedule result = await _schedulesMethods.GetScheduleByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<SchedulesController>/create
        /// <summary>
        /// Creates a new Schedule
        /// </summary>
        /// <param name="schedule">A Schedule object</param>
        /// <returns>Returns the newly created Schedule object</returns>
        [HttpPost("create")]
        public async Task<ActionResult<Schedule>> Post([FromBody] Schedule schedule)
        {
            if (schedule == null)
                return BadRequest();

            Schedule createdSchedule = await _schedulesMethods.CreateScheduleAsync(schedule);

            return Ok(createdSchedule);
        }

        // PUT api/<SchedulesController>/5
        /// <summary>
        /// Edits a specific node
        /// </summary>
        /// <param name="id">The Schedule id</param>
        /// <param name="modifiedSchedule">The modified Schedule object</param>
        /// <returns>Returns the modified Schedule object if successful</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<Schedule>> Put(int id, [FromBody] Schedule modifiedSchedule)
        {
            if (modifiedSchedule == null)
                return BadRequest();

            Schedule res = await _schedulesMethods.EditScheduleAsync(id, modifiedSchedule);
            if (res == null)
                return NotFound();

            return res;
        }

        // DELETE api/<SchedulesController>/5
        /// <summary>
        /// Deletes the Schedule identified by the gived id
        /// </summary>
        /// <param name="id">The Schedule id</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool? res = await _schedulesMethods.DeleteScheduleAsync(id);
            if (res == null)
                return NotFound();

            return Ok();
        }
    }
}
