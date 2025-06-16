using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class NewaddedFloat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "ImageSize",
                table: "Nodes",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ImageSize",
                table: "Nodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
