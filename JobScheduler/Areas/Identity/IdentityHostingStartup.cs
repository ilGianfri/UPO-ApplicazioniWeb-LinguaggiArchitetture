using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(JobScheduler.Areas.Identity.IdentityHostingStartup))]
namespace JobScheduler.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}