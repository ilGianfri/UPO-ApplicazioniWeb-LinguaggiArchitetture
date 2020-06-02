using JobScheduler.Data;
using JobScheduler.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public SchedulesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<SchedulesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> Get()
        {
            List<Job> schedules = await _dbContext.Jobs.ToListAsync();
            return schedules == null ? new EmptyResult() : (ActionResult<IEnumerable<Schedule>>)Ok(schedules);
        }

        // GET api/<SchedulesController>/5
        [HttpGet("{id}")]
        public ActionResult<Schedule> Get(int id)
        {
            Schedule result = _dbContext.Schedules.FirstOrDefault(x => x.Id == id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<SchedulesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Schedule schedule)
        {
            if (schedule == null)
                return BadRequest();

            _dbContext.Schedules.Add(schedule);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<SchedulesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Schedule>> Put(int id, [FromBody] Schedule modifiedSchedule)
        {
            if (modifiedSchedule == null)
                return BadRequest();

            Schedule schedule = _dbContext.Schedules.FirstOrDefault(x => x.Id == id);
            if (schedule != null)
            {
                schedule = modifiedSchedule;
                await _dbContext.SaveChangesAsync();

                return Ok(schedule);
            }

            return NotFound();
        }

        // DELETE api/<SchedulesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _dbContext.Schedules.Remove(_dbContext.Schedules.FirstOrDefault(x => x.Id == id));
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
