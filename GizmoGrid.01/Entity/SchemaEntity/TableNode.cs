namespace GizmoGrid._01.Entity.SchemaEntity
{
    public class TableNode
    {
        public Guid TableNodeId { get; set; }
        public string TableName { get; set; }
       public string DatabaseEngine {  get; set; }  
         public string  DatabaseName { get; set; } 
        public Guid SchemaDiagramId { get; set; }
        public SchemaDiagram SchemaDiagram { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public ICollection<TableColumn> Columns { get; set; }         
        public ICollection<TableEdges> OutgoingEdges { get; set; }      
        public ICollection<TableEdges> IncomingEdges { get; set; }

    }
}
    