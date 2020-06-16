using System;
using System.Collections.Generic;

namespace JobScheduler.Shared
{
    public partial class Jobs
    {
        public Jobs()
        {
            Nodes = new HashSet<Node>();
            Schedules = new HashSet<Schedule>();
        }

        public long Id { get; set; }
        public string Path { get; set; }
        public string Parameters { get; set; }
        public long Status { get; set; }

        public virtual ICollection<Node> Nodes { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}