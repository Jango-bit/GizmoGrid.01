using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class Schema122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatabaseEngine",
                table: "TableNodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DatabaseName",
                table: "TableNodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "TableNodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseEngine",
                table: "TableNodes");

            migrationBuilder.DropColumn(
                name: "DatabaseName",
                table: "TableNodes");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "TableNodes");
        }
    }
}
