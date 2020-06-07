using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Models
{
    public class Schedule
    {
        public Schedule()
        {
            Job = new Job();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime When { get; set; }
        public Job Job { get; set;  }
        public string Cron { get; set; }
    }
}
