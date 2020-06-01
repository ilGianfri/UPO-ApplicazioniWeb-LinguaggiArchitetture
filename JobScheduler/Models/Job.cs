﻿namespace JobScheduler.Models
{
    public class Job
    {
        public ulong Id { get; set; }
        public string Path { get; set; }
        public string Parameters { get; set; }
        public Node[] Nodes { get; set; }
        public JobStatus Status { get; set; }
        
    }

    public enum JobStatus
    {
        Scheduled,
        Running,
        Waiting,
        Stopped
    }
}
