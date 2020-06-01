using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Models
{
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }
        public string Path { get; set; }
        public string Parameters { get; set; }
        public string[] Nodes { get; set; }
        public JobStatus Status { get; set; }
        
    }

    public enum JobStatus
    {
        Scheduled,
        Running,
        Waiting,
        Stopped
    }
}
