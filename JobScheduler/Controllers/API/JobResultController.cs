using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobResultController : ControllerBase
    {
        // GET: api/<JobResultController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
