namespace GizmoGrid._01.Dto.SchemaDto
{
    public class TableNodeDtoReturn
    {
        public Guid TableNodeId { get; set; }
        public string TableName { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseEngine { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public List<TableColumnDtoReturn> Columns { get; set; }
    }
}
