using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static JobScheduler.Areas.Identity.Pages.Account.LoginModel;

namespace JobScheduler.Controllers.API
{
    /// <summary>
    /// API to get a JWT Token
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly TokenMethods _tokenMethods;

        public TokensController(TokenMethods tokenMethods)
        {
            _tokenMethods = tokenMethods;
        }

        /// <summary>
        /// Creates a token for the specified user
        /// </summary>
        /// <param name="model">The user details</param>
        /// <returns>A JwtToken object containing the user token</returns>
        [HttpPost]
        public async Task<ActionResult<JwtToken>> CreateToken([FromBody] InputModel model)
        {
            if (ModelState.IsValid)
            {
                JwtToken token = await _tokenMethods.CreateToken(model);
                if (token != null)
                    return token;
            }

            return BadRequest();
        }
    }
}
