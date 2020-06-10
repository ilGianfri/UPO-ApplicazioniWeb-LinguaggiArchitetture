using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    /// <summary>
    /// This model is used to save the result of a Job
    /// </summary>
    public class JobReports
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }
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
