using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool IsChecked { get; set; }
        [JsonIgnore]
        public virtual ICollection<GroupNode> GroupNodes { get; set; } = new HashSet<GroupNode>();
    }
}
