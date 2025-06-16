namespace GizmoGrid._01.Entity
{

    public class Edge
    {

        public Guid Id { get; set; }

        public Guid SourceId { get; set; }
        public Node SourceNode { get; set; }  // ✅ Navigation property

        public Guid TargetId { get; set; }
        public Node TargetNode { get; set; }  // ✅ Navigation property

        public string SourceHandle { get; set; } = "default";
        public string TargetHandle { get; set; } = "default";
        public Guid FlowDiagramId { get; set; }
        public FlowDiagram FlowDiagram { get; set; }
        
    }
}
