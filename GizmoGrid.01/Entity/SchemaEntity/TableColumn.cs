namespace GizmoGrid._01.Entity.SchemaEntity
{
    public class TableColumn
    {
        // public Guid TableColumnId { get; set; } = Guid.NewGuid();
        //public string ColumnName { get; set; }
        // public string DataType { get; set; }

        // public bool IsPrimaryKey { get; set; }
        // public bool IsNullable { get; set; }
        // public bool IsUnique { get; set; }
        // public string? DefaultValue { get; set; }

        // // Foreign key to TableNode
        // public Guid TableNodeId { get; set; }
        // public TableNode TableNode { get; set; }

        // public Guid? ForeignKeyReferenceColumnId { get; set; }
        // public TableColumn? ForeignKeyReferenceColumn { get; set; }
        // public string? ForeignKeyTable { get; set; }
        // public string? ForeignKeyColumn { get; set; }


        public Guid TableColumnId { get; set; } = Guid.NewGuid();

        public string ColumnName { get; set; }
        public string DataType { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; }
        public string? DefaultValue { get; set; }

        // Link to parent TableNode
        public Guid TableNodeId { get; set; }
        public TableNode TableNode { get; set; }

        // Actual foreign key relationship to another column
        public Guid? ForeignKeyReferenceColumnId { get; set; }
        public TableColumn? ForeignKeyReferenceColumn { get; set; }

        // Visual aid (used only in UI or import scenarios)
        public string? ForeignKeyTable { get; set; }   // Name of referenced table
        public string? ForeignKeyColumn { get; set; }
    }
}
