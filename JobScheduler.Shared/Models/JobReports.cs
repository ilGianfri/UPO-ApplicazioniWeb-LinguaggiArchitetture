using System;
using System.Collections.Generic;

namespace JobScheduler.Shared
{
    public partial class JobReports
    {
        public long Id { get; set; }
        public long? JobId { get; set; }
        public long? ExitCode { get; set; }
        public long Pid { get; set; }
        public string Output { get; set; }
    }
}