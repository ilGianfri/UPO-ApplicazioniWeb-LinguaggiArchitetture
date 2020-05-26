using System.Net;

namespace JobScheduler.Data
{
    public class Node
    {
        public IPAddress IP { get; set; }
        public int Group { get; set; }
    }
}
