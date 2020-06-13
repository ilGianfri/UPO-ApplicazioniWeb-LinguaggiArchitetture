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
        public DbSet<JobReport> JobReports { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Node>().ToTable("Nodes").HasKey(x => x.Id);
            builder.Entity<Job>().ToTable("Jobs").HasKey(x => x.Id);
            builder.Entity<Schedule>().ToTable("Schedules").HasKey(x => x.Id);
            builder.Entity<JobReport>().ToTable("JobReports").HasKey(x => x.Id);
            builder.Entity<Group>().ToTable("Groups").HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}
