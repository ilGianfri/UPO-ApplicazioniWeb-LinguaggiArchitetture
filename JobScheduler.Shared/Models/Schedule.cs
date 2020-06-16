using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public partial class Schedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("JobId")]
        public int? JobId { get; set; }
        [JsonPropertyName("Cron")]
        public string Cron { get; set; }
        [NotMapped]
        [JsonIgnore]
        public DateTime When { get; set; }
        [JsonPropertyName("Job")]
        public virtual Job Job { get; set; }
    }
}