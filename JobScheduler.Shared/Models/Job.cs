using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Shared.Models
{
    public partial class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Path { get; set; }
        public string Parameters { get; set; }
        public JobStatus Status { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
        public virtual ICollection<Node> Nodes { get; set; } = new HashSet<Node>();
    }

    public enum JobStatus
    {
        Scheduled,
        Running,
        Waiting,
        Stopped
    }
}