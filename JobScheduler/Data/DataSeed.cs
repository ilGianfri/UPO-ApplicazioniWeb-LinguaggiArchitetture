﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace JobScheduler.Data
{
    public class DataSeed
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private IConfiguration Configuration { get; }

        public DataSeed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            Configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            const string email = "admin@jobscheduler.com";
            string password = Configuration.GetValue<string>("DefaultAdminPassword");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded) throw new InvalidOperationException("Cannot create default user");
            }

            if (! await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("Editor"))
                await _roleManager.CreateAsync(new IdentityRole("Editor"));

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded) throw new InvalidOperationException("Cannot add role Admin to default user");
            }
        }
    }
}