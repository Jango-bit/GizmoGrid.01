namespace GizmoGrid._01.Dto.SchemaDto
{
    public class TableNodeUpdateDto
    {
        public string TableName { get; set; } = string.Empty;
        public string DatabaseEngine { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public float PositionX { get; set; }
        public float PositionY { get; set; }

        public List<TableColumnUpdateDto> Columns { get; set; } = new();
    }
}
