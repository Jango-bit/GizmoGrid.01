using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class InitiialaazenEWwee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Edges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlowDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Edges_FlowDiagrams_FlowDiagramId",
                        column: x => x.FlowDiagramId,
                        principalTable: "FlowDiagrams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Edges_Nodes_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Edges_Nodes_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Nodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Edges_FlowDiagramId",
                table: "Edges",
                column: "FlowDiagramId");

            migrationBuilder.CreateIndex(
                name: "IX_Edges_SourceId",
                table: "Edges",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Edges_TargetId",
                table: "Edges",
                column: "TargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Edges");
        }
    }
}
