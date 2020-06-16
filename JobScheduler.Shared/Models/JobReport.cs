using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public partial class JobReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("JobId")]
        public int? JobId { get; set; }
        [JsonPropertyName("ExitCode")]
        public int? ExitCode { get; set; }
        [JsonPropertyName("Pid")]
        public long Pid { get; set; }
        [JsonPropertyName("Output")]
        public string Output { get; set; }
    }
}