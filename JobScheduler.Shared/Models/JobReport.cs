using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    /// <summary>
    /// This model is used to save the result of a Job
    /// </summary>
    public class JobReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("JobId")]
        public string JobId { get; set; }
        [JsonPropertyName("ExitCode")]
        public string ExitCode { get; set; }
        [JsonPropertyName("Pid")]
        public int Pid { get; set; }
        [JsonPropertyName("Output")]
        public string Output { get; set; }
    }
}
