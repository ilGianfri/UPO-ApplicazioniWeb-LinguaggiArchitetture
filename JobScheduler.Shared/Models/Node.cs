using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public partial class Node
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("IPStr")]
        public string IPStr { get; set; }
        [JsonPropertyName("Port")]
        public int Port { get; set; }
        [JsonPropertyName("Role")]
        public NodeRole Role { get; set; }
        [JsonPropertyName("JobId")]
        public int? JobId { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool IsChecked { get; set; }
        [JsonPropertyName("Job")]
        public virtual Job Job { get; set; }
        [JsonIgnore]
        public virtual ICollection<GroupNode> GroupNodes { get; set; } = new HashSet<GroupNode>();
    }

    public enum NodeRole
    {
        Master,
        Slave
    }
}