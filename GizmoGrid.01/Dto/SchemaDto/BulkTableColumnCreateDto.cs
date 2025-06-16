namespace GizmoGrid._01.Dto.SchemaDto
{
    public class BulkTableColumnCreateDto
    {
        public List<TableColumnCreateDto> Columns { get; set; } = new();
    }
}
