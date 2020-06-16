using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Shared.Models
{
    public partial class JobReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? JobId { get; set; }
        public int? ExitCode { get; set; }
        public long Pid { get; set; }
        public string Output { get; set; }
    }
}