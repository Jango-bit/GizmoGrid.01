using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Entity
{
    public class Node
    {
     

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FlowDiagramId { get; set; }
        public FlowDiagram? FlowDiagram { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public float? ImageSize { get; set; } // Scaling factor
        public float PositionX { get; set; }
        public float PositionY { get; set; }

    public ICollection<Edge> IncomingEdges { get; set; } = new List<Edge>();  // ✅ All edges pointing TO this node
    public ICollection<Edge> OutgoingEdges { get; set; } = new List<Edge>(); 

    }

}
