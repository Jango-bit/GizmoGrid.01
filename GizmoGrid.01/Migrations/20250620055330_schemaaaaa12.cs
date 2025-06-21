using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class schemaaaaa12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableNodes_SchemaDiagrams_SchemaDiagramId",
                table: "TableNodes");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "SchemaDiagrams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchemaDiagrams_ProjectId",
                table: "SchemaDiagrams",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SchemaDiagrams_ProjectId1",
                table: "SchemaDiagrams",
                column: "ProjectId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApiDiagrams_ProjectId",
                table: "ApiDiagrams",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiDiagrams_Projects_ProjectId",
                table: "ApiDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId",
                table: "SchemaDiagrams",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId1",
                table: "SchemaDiagrams",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TableNodes_SchemaDiagrams_SchemaDiagramId",
                table: "TableNodes",
                column: "SchemaDiagramId",
                principalTable: "SchemaDiagrams",
                principalColumn: "SchemaDiagramId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiDiagrams_Projects_ProjectId",
                table: "ApiDiagrams");

            migrationBuilder.DropForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId",
                table: "SchemaDiagrams");

            migrationBuilder.DropForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId1",
                table: "SchemaDiagrams");

            migrationBuilder.DropForeignKey(
                name: "FK_TableNodes_SchemaDiagrams_SchemaDiagramId",
                table: "TableNodes");

            migrationBuilder.DropIndex(
                name: "IX_SchemaDiagrams_ProjectId",
                table: "SchemaDiagrams");

            migrationBuilder.DropIndex(
                name: "IX_SchemaDiagrams_ProjectId1",
                table: "SchemaDiagrams");

            migrationBuilder.DropIndex(
                name: "IX_ApiDiagrams_ProjectId",
                table: "ApiDiagrams");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "SchemaDiagrams");

            migrationBuilder.AddForeignKey(
                name: "FK_TableNodes_SchemaDiagrams_SchemaDiagramId",
                table: "TableNodes",
                column: "SchemaDiagramId",
                principalTable: "SchemaDiagrams",
                principalColumn: "SchemaDiagramId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
