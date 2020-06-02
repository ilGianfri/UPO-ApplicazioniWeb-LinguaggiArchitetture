using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Models
{
    public class Schedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime When { get; set; }
    }
}
