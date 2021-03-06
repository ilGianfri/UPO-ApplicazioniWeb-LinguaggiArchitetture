﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public partial class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Path")]
        public string Path { get; set; }
        [JsonPropertyName("Parameters")]
        public string Parameters { get; set; }
        [JsonPropertyName("Status")]
        public JobStatus Status { get; set; }
        [JsonPropertyName("GroupId")]
        public int? GroupId { get; set; }
        [JsonPropertyName("Group")]
        public virtual Group Group { get; set; }
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
    }

    public enum JobStatus
    {
        Scheduled,
        Running,
        Waiting,
        Stopped,
        Exited
    }
}