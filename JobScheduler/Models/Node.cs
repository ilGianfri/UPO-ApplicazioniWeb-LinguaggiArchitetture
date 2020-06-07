using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace JobScheduler.Models
{
    public class Node
    {
        public Node() { }

        public Node(int id, string name, int[] group, NodeRole role)
        {
            Id = id;
            Name = name;
            Group = group;
            Role = role;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string IPStr { get; set; } = string.Empty;

        [NotMapped]
        public IPAddress IP { get; set; }
        public int[] Group { get; set; }
        public NodeRole Role { get; set; }
    }

    public enum NodeRole
    {
        Master,
        Slave
    }
}
