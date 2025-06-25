using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class projectdt111222222okikh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId",
                table: "SchemaDiagrams");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId",
                table: "SchemaDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId",
                table: "SchemaDiagrams");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId",
                table: "SchemaDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
