using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class Edgeschange123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyColumn",
                table: "TableColumns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTable",
                table: "TableColumns",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyColumn",
                table: "TableColumns");

            migrationBuilder.DropColumn(
                name: "ForeignKeyTable",
                table: "TableColumns");
        }
    }
}
