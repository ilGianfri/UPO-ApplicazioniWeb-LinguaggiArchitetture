using JobScheduler.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupNode> GroupNodes { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobReport> JobReports { get; set; }
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GroupNode>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.NodeId });

                entity.HasIndex(e => e.NodeId);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupNodes)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Node)
                    .WithMany(p => p.GroupNodes)
                    .HasForeignKey(d => d.NodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasIndex(e => e.GroupId);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.GroupId);
            });

            modelBuilder.Entity<JobReport>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Node>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IPStr).HasColumnName("IPStr");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasIndex(e => e.JobId);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.JobId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}