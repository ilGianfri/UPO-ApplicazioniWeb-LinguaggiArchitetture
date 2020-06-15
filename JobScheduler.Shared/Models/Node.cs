﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace JobScheduler.Shared.Models
{
    public class Node
    {
        public Node() { }
        public Node(int id, string name, List<Group> group, NodeRole role)
        {
            Id = id;
            Name = name;
            Group = group;
            Role = role;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("IPStr")]
        public string IPStr { get; set; }
        [JsonPropertyName("Port")]
        public int Port { get; set; }

        [NotMapped]
        [JsonPropertyName("IP")]
        public IPAddress IP { get; set; } = null;
        [JsonPropertyName("Group")]
        public List<Group> Group { get; set; } = new List<Group>();
        [JsonPropertyName("Role")]
        public NodeRole Role { get; set; }
        [JsonIgnore]
        [NotMapped]
        public bool IsChecked { get; set; }
    }

    public enum NodeRole
    {
        Master,
        Slave
    }
}
