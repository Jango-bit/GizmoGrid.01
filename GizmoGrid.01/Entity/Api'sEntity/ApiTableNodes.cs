namespace GizmoGrid._01.Entity.Api_sEntity
{
    public class ApiTableNodes
    {
        public Guid ApiTableNodesId { get; set; }
        public Guid ApiDiagramId { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public ICollection<ApiEdges>? IncomingEdges { get; set; } = new List<ApiEdges>();
        public ICollection<ApiEdges>? OutgoingEdges { get; set; } = new List<ApiEdges>();


    }
}
