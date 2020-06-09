using JobScheduler.Data;
using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Editor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SchedulesController : ControllerBase
    {
        private ApplicationDbContext _dbContext;
        private SchedulesMethods _schedulesMethods;
        public SchedulesController(ApplicationDbContext dbContext, SchedulesMethods schedulesMethods)
        {
            _dbContext = dbContext;
            _schedulesMethods = schedulesMethods;
        }

        // GET: api/<SchedulesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> Get()
        {
            List<Schedule> schedules = (await _schedulesMethods.GetSchedulesAsync()).ToList();

            return Ok(schedules);
        }

        // GET api/<SchedulesController>/5
        [HttpGet("{id}")]
        public ActionResult<Schedule> Get(int id)
        {
            Schedule result = _schedulesMethods.GetScheduleByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<SchedulesController>
        [HttpPost]
        public async Task<ActionResult<Schedule>> Post([FromBody] Schedule schedule)
        {
            if (schedule == null)
                return BadRequest();

            var createdSchedule = await _schedulesMethods.CreateScheduleAsync(schedule);

            return Ok(createdSchedule);
        }

        // PUT api/<SchedulesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Schedule>> Put(int id, [FromBody] Schedule modifiedSchedule)
        {
            if (modifiedSchedule == null)
                return BadRequest();

            var res = await _schedulesMethods.EditScheduleAsync(id, modifiedSchedule);
            if (res == null)
                return NotFound();

            return res;
        }

        // DELETE api/<SchedulesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var res = await _schedulesMethods.DeleteScheduleAsync(id);
            if (res == null)
                return NotFound();

            return Ok();
        }
    }
}
