using System;

namespace JobScheduler.Models
{
    public class Schedule
    {
        public ulong Id { get; set; }
        public DateTime When { get; set; }
    }
}
