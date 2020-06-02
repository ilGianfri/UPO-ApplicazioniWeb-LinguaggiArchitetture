using JobScheduler.Data;
using JobScheduler.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public ActionResult<string> Get(int id)
        {
            return Ok("value");
        }

        // POST api/<SchedulesController>
        [HttpPost]
        public ActionResult Post([FromBody] Schedule value)
        {
            return Ok();
        }

        // PUT api/<SchedulesController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Schedule value)
        {
            return Ok();
        }

        // DELETE api/<SchedulesController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
