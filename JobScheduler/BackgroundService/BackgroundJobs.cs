using System;
using System.Threading.Tasks;
using System.Threading;
using JobScheduler.Data;

namespace JobScheduler.BackgroundService
{
    public class BackgroundJobs : Microsoft.Extensions.Hosting.BackgroundService
    {
        private ApplicationDbContext _dbContext;
        public BackgroundJobs(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
