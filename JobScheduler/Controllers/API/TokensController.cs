﻿using System.Threading.Tasks;
using JobScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using static JobScheduler.Areas.Identity.Pages.Account.LoginModel;

namespace JobScheduler.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private TokenMethods _tokenMethods;

        public TokensController(TokenMethods tokenMethods)
        {
            _tokenMethods = tokenMethods;
        }

        /// <summary>
        /// Creates a token for the users
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<JwtToken>> CreateToken([FromBody] InputModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _tokenMethods.CreateToken(model);
                if (token != null)
                    return token;
            }

            return BadRequest();
        }
    }
}
