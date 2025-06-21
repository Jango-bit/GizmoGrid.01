using System.Xml.Linq;
using GizmoGrid._01.Entity.Api_sEntity;
using GizmoGrid._01.Entity.SchemaEntity;

namespace GizmoGrid._01.Entity
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<FlowDiagram> FlowDiagrams { get; set; }
        public List<SchemaDiagram> SchemaDiagrams { get; set; }

        public List<ApiDiagram> ApiDiagrams { get; set; }   
    }
}
