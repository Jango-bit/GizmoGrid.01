namespace GizmoGrid._01.Entity
{
    public class FlowDiagram
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Label { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        public ICollection<Edge> Edges { get; set; } = new List<Edge>();


    }

}
