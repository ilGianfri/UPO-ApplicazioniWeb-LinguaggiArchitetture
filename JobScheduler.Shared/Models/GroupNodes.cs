namespace JobScheduler.Shared
{
    public partial class GroupNodes
    {
        public long GroupId { get; set; }
        public long NodeId { get; set; }

        public virtual Groups Group { get; set; }
        public virtual Nodes Node { get; set; }
    }
}