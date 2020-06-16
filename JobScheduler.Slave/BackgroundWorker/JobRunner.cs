using JobScheduler.Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobScheduler.Slave.BackgroundWorker
{
    public class JobRunner
    {
        public static async Task ExecuteAsync(Job job, CancellationToken stoppingToken)
        {
            //TODO
        }

        private static void JobProcess_Exited(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
