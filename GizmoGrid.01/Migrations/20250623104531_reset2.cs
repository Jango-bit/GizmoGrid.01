using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class reset2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId1",
                table: "SchemaDiagrams");

            migrationBuilder.DropIndex(
                name: "IX_SchemaDiagrams_ProjectId1",
                table: "SchemaDiagrams");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "SchemaDiagrams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "SchemaDiagrams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchemaDiagrams_ProjectId1",
                table: "SchemaDiagrams",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemaDiagrams_Projects_ProjectId1",
                table: "SchemaDiagrams",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
