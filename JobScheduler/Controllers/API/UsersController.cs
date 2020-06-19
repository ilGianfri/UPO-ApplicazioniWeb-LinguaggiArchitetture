using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobScheduler.Controllers.API
{
    /// <summary>
    /// CRUD API for Users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly UserMethods _userMethods;
        public UsersController(UserMethods userMethods)
        {
            _userMethods = userMethods;
        }

        // GET: api/<UsersController>
        /// <summary>
        /// Returns all the users with their assigned role.
        /// </summary>
        /// <returns>Returns a IEnumerable of UserWithRole objects. Each object contains the User object and its Role</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWithRole>>> Get()
        {
            IEnumerable<UserWithRole> result = await _userMethods.GetUsersAsync();
            if (result != null)
                return Ok(result);

            return NotFound();
        }

        // GET api/<UsersController>/5
        /// <summary>
        /// Returns a user given its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        // POST api/<UsersController>/create
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="newUser">A UserWithRole object containing the new user details (and the Role to assign him)</param>
        [HttpPost("create")]
        public async Task<ActionResult> PostCreate([FromBody] UserWithRole newUser)
        {
            if (newUser == null)
                return BadRequest();

            bool result = await _userMethods.CreateUserAsync(newUser);
            if (result)
                return StatusCode(201);

            return StatusCode(500);
        }

        // PUT api/<UsersController>/5
        /// <summary>
        /// Edits an existing user
        /// </summary>
        /// <param name="id">The id of the user to modify</param>
        /// <param name="modifiedUser">A UserWithRole object containing the modified user details</param>
        /// <returns>Returns the modified user object if successfull</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserWithRole>> Put(string id, [FromBody] UserWithRole modifiedUser)
        {
            if (modifiedUser == null)
                return BadRequest();

            UserWithRole edited = await _userMethods.EditUserAsync(id, modifiedUser);
            if (edited != null)
            {
                return Ok(edited);
            }

            return NotFound();
        }

        // DELETE api/<UsersController>/5
        /// <summary>
        /// Deletes the specified user
        /// </summary>
        /// <param name="id">The id of the user to delete</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            bool? deleted = await _userMethods.DeleteUserAsync(id);
            if (deleted != null)
            {
                if (deleted.HasValue)
                    return Ok();
            }

            return NotFound();
        }
    }
}
