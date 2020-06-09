using System.Collections.Generic;
using System.Threading.Tasks;
using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserMethods _userMethods;
        public UsersController(UserManager<IdentityUser> userManager, UserMethods userMethods)
        {
            _userManager = userManager;
            _userMethods = userMethods;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithRole>>> Get()
        {
            var result = await _userMethods.GetUsersAsync();
            if (result != null)
                return Ok(result);

            return NotFound();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithRole>> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            UserWithRole user = await _userMethods.GetUserByIdAsync(id);
            if (user != null)
                return Ok(user);

            return NotFound();
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> PostCreate([FromBody] UserWithRole newUser)
        {
            if (newUser == null)
                return BadRequest();

            var result = await _userMethods.CreateUserAsync(newUser);
            if (result)
                return StatusCode(201);

            return StatusCode(500);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserWithRole modifiedUser)
        {
            if (modifiedUser == null)
                return BadRequest();

            var edited = await _userMethods.EditUserAsync(id, modifiedUser);
            if (edited != null)
            {
                return Ok();
            }

            return NotFound();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var deleted = await _userMethods.DeleteUserAsync(id);
            if (deleted != null)
            {
                if (deleted.HasValue)
                    return Ok();
            }

            return NotFound();
        }
    }
}
