namespace GizmoGrid._01.Dto.SchemaDto
{
    public class TableColumnUpdateDto
    {
        public Guid TableColumnId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; }
        public string? DefaultValue { get; set; }
    }
}
