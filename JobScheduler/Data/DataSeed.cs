using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace JobScheduler.Data
{
    public class DataSeed
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        private IConfiguration Configuration { get; }

        public DataSeed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            Configuration = configuration;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            const string email = "admin@jobscheduler.com";
            string password = Configuration.GetValue<string>("DefaultAdminPassword");
            const string username = "admin";

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = email
                };

                IdentityResult result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Cannot create default user");
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _roleManager.RoleExistsAsync("Editor"))
                await _roleManager.CreateAsync(new IdentityRole("Editor"));

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!roleResult.Succeeded)
                    throw new InvalidOperationException("Cannot add role Admin to default user");
            }

            if (_dbContext.Nodes.FirstOrDefault(x => x.IPStr == GetIPAddress()) == null)
            {
                _dbContext.Nodes.Add(new Node() { IPStr = GetIPAddress(), Name = "Master", Role = NodeRole.Master });
                await _dbContext.SaveChangesAsync();
            }
        }

        private string GetIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
        }
    }
}