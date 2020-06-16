using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class Group
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        [JsonIgnore]
        public bool IsChecked { get; set; }
        public virtual ICollection<GroupNode> GroupNodes { get; set; } = new HashSet<GroupNode>();
    }
}
