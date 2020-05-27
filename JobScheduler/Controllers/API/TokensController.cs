using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JobScheduler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static JobScheduler.Areas.Identity.Pages.Account.LoginModel;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokensController(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] InputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.Id.ToString())
                            }),
                            Expires = DateTime.UtcNow.AddYears(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        foreach (var role in await _userManager.GetRolesAsync(user))
                        {
                            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                        }

                        var token = tokenHandler.CreateToken(tokenDescriptor);

                        return Ok(new JwtToken
                        {
                            Token = tokenHandler.WriteToken(token),
                            Expiration = token.ValidTo,
                        });
                    }
                }
            }

            return BadRequest();
        }
    }
}
