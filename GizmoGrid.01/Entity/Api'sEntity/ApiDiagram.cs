using GizmoGrid._01.Entity.SchemaEntity;

namespace GizmoGrid._01.Entity.Api_sEntity
{
    public class ApiDiagram
    {
        public Guid ApiDiagramId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }    
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<ApiTableNodes> ApiTableNodes { get; set; } = new List<ApiTableNodes>();
        public ICollection<ApiEdges> ApiEdges { get; set; } = new List<ApiEdges>();

    }
}
