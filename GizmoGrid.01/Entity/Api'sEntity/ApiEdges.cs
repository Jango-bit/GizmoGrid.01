    using GizmoGrid._01.Entity.SchemaEntity;

    namespace GizmoGrid._01.Entity.Api_sEntity
    {
        public class ApiEdges
        {
            public Guid ApiEdgesId { get; set; } = Guid.NewGuid();
            public Guid ApiDiagramId { get; set; }
            public ApiDiagram ApiDiagram { get; set; }

            public Guid SourceId { get; set; }
            public Guid TargetId { get; set; }

        public ApiTableNodes SourceNode { get; set; } 
        public ApiTableNodes TargetNode { get; set; } 
    }
    }
