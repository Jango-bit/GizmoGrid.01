using GizmoGrid._01.Entity;

namespace GizmoGrid._01.Dto
{
    public class HistoryCreateDto
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int HistoryIndex { get; set; }
    }
}
