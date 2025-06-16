namespace GizmoGrid._01.Dto.SchemaDto
{
    public class TableColumnDtoReturn
    {
        public Guid ColumnId { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; }
        public string? DefaultValue { get; set; }
        public ForeignKeyReferenceDto? ForeignKey { get; set; }
    }
}
