﻿using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static JobScheduler.Areas.Identity.Pages.Account.LoginModel;

#nullable enable

namespace JobScheduler.Controllers
{
    public class TokenMethods
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenMethods(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a token for the specified user
        /// </summary>
        /// <param name="model">The user details</param>
        /// <returns>A JwtToken object containing the user token</returns>
        public async Task<JwtToken?> CreateToken(InputModel model)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    JwtSecurityTokenHandler? tokenHandler = new JwtSecurityTokenHandler();
                    byte[]? key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);
                    SecurityTokenDescriptor? tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddMonths(2),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    foreach (string? role in await _userManager.GetRolesAsync(user))
                    {
                        tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
                    }

                    SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

                    return new JwtToken
                    {
                        Token = tokenHandler.WriteToken(token),
                        Expiration = token.ValidTo,
                    };
                }
            }
            return null;
        }
    }
}
