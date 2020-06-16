using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Shared.Models
{
    public partial class Schedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? JobId { get; set; }
        public string Cron { get; set; }
        [NotMapped]
        public DateTime When { get; set; }
        public virtual Job Job { get; set; }
    }
}