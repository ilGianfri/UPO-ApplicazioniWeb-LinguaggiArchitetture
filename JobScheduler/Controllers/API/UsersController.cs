using System.Collections.Generic;
using System.Threading.Tasks;
using JobScheduler.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithRole>>> Get()
        {
            List<IdentityUser> users = await _userManager.Users.ToListAsync();
            if (users == null)
                return StatusCode(500);

            List<UserWithRole> result = new List<UserWithRole>();
            foreach (IdentityUser user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserWithRole() { User = user, Roles = roles });
            }
            return result;
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithRole>> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            IdentityUser user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                UserWithRole result = new UserWithRole() { User = user, Roles = roles };

                return result;
            }

            return NotFound();
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> PostCreate([FromBody] UserWithRole newUser)
        {
            if (newUser == null)
                return BadRequest();

            var result = await _userManager.CreateAsync(newUser.User, newUser.User.PasswordHash);
            if (result.Succeeded)
            {
                foreach (string role in newUser.Roles)
                    await _userManager.AddToRoleAsync(newUser.User, role);
                return StatusCode(201);
            }
            return BadRequest();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                return deleteResult.Succeeded ? Ok() : (IActionResult)BadRequest();
            }

            return NotFound();
        }
    }
}
