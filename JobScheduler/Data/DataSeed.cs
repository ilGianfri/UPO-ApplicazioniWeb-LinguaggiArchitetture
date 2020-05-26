using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace JobScheduler.Data
{
    public class DataSeed
    {
        private readonly UserManager<IdentityUser> _userManager;
        private IConfiguration Configuration { get; }

        public DataSeed(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            Configuration = configuration;
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
        }
    }
}