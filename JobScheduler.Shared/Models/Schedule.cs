using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class Schedule
    {
        //public Schedule()
        //{
        //    Job = new Job();
        //}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("When")]
        [NotMapped]
        public DateTime When { get; set; }
        [JsonPropertyName("Job")]
        public Job Job { get; set; }
        [JsonPropertyName("Cron")]
        public string Cron { get; set; }
    }
}
