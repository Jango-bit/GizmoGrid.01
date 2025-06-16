namespace GizmoGrid._01.Entity.SchemaEntity
{
    public class SchemaDiagram
    {
        public Guid SchemaDiagramId { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<TableNode> TableNodes { get; set; } = new List<TableNode>();

    }
}
