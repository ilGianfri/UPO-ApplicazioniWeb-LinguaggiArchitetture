namespace JobScheduler.Shared.Models
{
    public partial class GroupNode
    {
        public int GroupId { get; set; }
        public int NodeId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Node Node { get; set; }
    }
}