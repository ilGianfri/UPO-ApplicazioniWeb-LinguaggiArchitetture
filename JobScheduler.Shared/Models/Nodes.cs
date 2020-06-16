using System;
using System.Collections.Generic;

namespace JobScheduler.Shared
{
    public partial class Nodes
    {
        public Nodes()
        {
            GroupNodes = new HashSet<GroupNodes>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string IPStr { get; set; }
        public long Port { get; set; }
        public long Role { get; set; }
        public long? JobId { get; set; }

        public virtual Jobs Job { get; set; }
        public virtual ICollection<GroupNodes> GroupNodes { get; set; }
    }
}