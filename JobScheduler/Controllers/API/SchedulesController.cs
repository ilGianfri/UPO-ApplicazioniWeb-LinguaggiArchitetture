using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobScheduler.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        // GET: api/<SchedulesController>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok(new string[] { "value1", "value2" });
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
