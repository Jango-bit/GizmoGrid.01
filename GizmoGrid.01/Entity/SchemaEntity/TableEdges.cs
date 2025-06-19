namespace GizmoGrid._01.Entity.SchemaEntity
{
    public class TableEdges
    {
        public Guid EdgeId { get; set; } = Guid.NewGuid();
        public Guid SchemaDiagramId { get; set; }
        public SchemaDiagram SchemaDiagram { get; set; }

        public Guid SourceId { get; set; }
        public Guid TargetId { get; set; }

        public TableNode SourceNode { get; set; }
        public TableNode TargetNode { get; set; }


    }
}
