using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobScheduler.Data
{
    public class Job
    {
        public string Path { get; set; }
        public string Parameters { get; set; }
        public Node[] Nodes { get; set; }
    }
}
