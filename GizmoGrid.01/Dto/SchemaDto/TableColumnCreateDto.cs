namespace GizmoGrid._01.Dto.SchemaDto
{
    public class TableColumnCreateDto
    {
        public string ColumnName { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        public bool IsPrimaryKey { get; set; }

        // This should represent "NOT NULL" = false if it's required
        public bool IsNullable { get; set; } = true;

        public bool IsUnique { get; set; }

        public string? DefaultValue { get; set; }

        public ForeignKeyReferenceDto? ForeignKey { get; set; }

    }
}
