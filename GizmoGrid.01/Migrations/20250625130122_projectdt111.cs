using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class projectdt111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiDiagrams_Projects_ProjectId",
                table: "ApiDiagrams");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowDiagrams_Projects_ProjectId",
                table: "FlowDiagrams");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiDiagrams_Projects_ProjectId",
                table: "ApiDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowDiagrams_Projects_ProjectId",
                table: "FlowDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiDiagrams_Projects_ProjectId",
                table: "ApiDiagrams");

            migrationBuilder.DropForeignKey(
                name: "FK_FlowDiagrams_Projects_ProjectId",
                table: "FlowDiagrams");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiDiagrams_Projects_ProjectId",
                table: "ApiDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FlowDiagrams_Projects_ProjectId",
                table: "FlowDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
