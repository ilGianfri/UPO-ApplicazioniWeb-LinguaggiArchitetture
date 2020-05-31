namespace JobScheduler.Models
{
    /// <summary>
    /// This model is used to save the result of a Job
    /// </summary>
    public class JobReports
    {
        public string Id { get; set; }
        public string JobId { get; set; }
        public string ExitCode { get; set; }
        public  int Pid { get; set; }
        public string Output { get; set; }
    }
}
