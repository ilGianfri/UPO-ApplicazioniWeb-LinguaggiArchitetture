using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Path")]
        public string Path { get; set; }
        [JsonPropertyName("Parameters")]
        public string Parameters { get; set; }
        [JsonPropertyName("Nodes")]
        public Node[] Nodes { get; set; }
        [JsonPropertyName("Status")]
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
