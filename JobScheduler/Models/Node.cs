using System.Net;

namespace JobScheduler.Models
{
    public class Node
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public IPAddress IP { get; set; }
        public int[] Group { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        Master,
        Slave
    }
}
