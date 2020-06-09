using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<JobReports> JobHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Node>().ToTable("Nodes").HasKey(x => x.Id);
            builder.Entity<Job>().ToTable("Jobs").HasKey(x => x.Id);
            builder.Entity<Schedule>().ToTable("Schedules").HasKey(x => x.Id);
            builder.Entity<JobReports>().ToTable("JobHistory").HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}
