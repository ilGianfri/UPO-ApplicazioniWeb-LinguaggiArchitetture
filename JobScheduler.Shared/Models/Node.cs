using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobScheduler.Shared.Models
{
    public partial class Node
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string IPStr { get; set; }
        public int Port { get; set; }
        public NodeRole Role { get; set; }
        public int? JobId { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool IsChecked { get; set; }
        public virtual Job Job { get; set; }
        public virtual ICollection<GroupNode> GroupNodes { get; set; } = new HashSet<GroupNode>();
    }

    public enum NodeRole
    {
        Master,
        Slave
    }
}