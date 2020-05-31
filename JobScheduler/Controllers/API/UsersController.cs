using System.Collections.Generic;
using System.Linq;
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
                result.Add(new UserWithRole() { User = user, Role = roles.FirstOrDefault() });
            }
            return Ok(result);
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
                UserWithRole result = new UserWithRole() { User = user, Role = roles.FirstOrDefault() };

                return Ok(result);
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
                await _userManager.AddToRoleAsync(newUser.User, newUser.Role);
                return StatusCode(201);
            }
            return StatusCode(500);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserWithRole modifiedUser)
        {
            if (modifiedUser == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                //Update the email
                if (user.Email != modifiedUser.User.Email)
                {
                    user.Email = modifiedUser.User.Email;
                    await _userManager.UpdateAsync(user);
                }

                //The password has been changed
                if (user.PasswordHash != modifiedUser.User.PasswordHash)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, resetToken, modifiedUser.User.PasswordHash);
                }

                //Role has been changed
                if (!await _userManager.IsInRoleAsync(user, modifiedUser.Role))
                {
                    //Remove old role
                    await _userManager.RemoveFromRoleAsync(user, (await _userManager.GetRolesAsync(user)).FirstOrDefault());
                    //Apply new one
                    await _userManager.AddToRoleAsync(user, modifiedUser.Role);
                }

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

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var deleteResult = await _userManager.DeleteAsync(user);
                return deleteResult.Succeeded ? Ok() : StatusCode(500);
            }

            return NotFound();
        }
    }
}
