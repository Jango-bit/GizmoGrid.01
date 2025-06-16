using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class ApiDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiDiagrams",
                columns: table => new
                {
                    ApiDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiDiagrams", x => x.ApiDiagramId);
                    table.ForeignKey(
                        name: "FK_ApiDiagrams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApiTableNodes",
                columns: table => new
                {
                    ApiTableNodesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionX = table.Column<float>(type: "real", nullable: false),
                    PositionY = table.Column<float>(type: "real", nullable: false),
                    ApiDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiTableNodes", x => x.ApiTableNodesId);
                    table.ForeignKey(
                        name: "FK_ApiTableNodes_ApiDiagrams_ApiDiagramId",
                        column: x => x.ApiDiagramId,
                        principalTable: "ApiDiagrams",
                        principalColumn: "ApiDiagramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiEdges",
                columns: table => new
                {
                    ApiEdgesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiEdges", x => x.ApiEdgesId);
                    table.ForeignKey(
                        name: "FK_ApiEdges_ApiDiagrams_ApiDiagramId",
                        column: x => x.ApiDiagramId,
                        principalTable: "ApiDiagrams",
                        principalColumn: "ApiDiagramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiEdges_ApiTableNodes_SourceId",
                        column: x => x.SourceId,
                        principalTable: "ApiTableNodes",
                        principalColumn: "ApiTableNodesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiEdges_ApiTableNodes_TargetId",
                        column: x => x.TargetId,
                        principalTable: "ApiTableNodes",
                        principalColumn: "ApiTableNodesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiDiagrams_UserId",
                table: "ApiDiagrams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiEdges_ApiDiagramId",
                table: "ApiEdges",
                column: "ApiDiagramId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiEdges_SourceId",
                table: "ApiEdges",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiEdges_TargetId",
                table: "ApiEdges",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiTableNodes_ApiDiagramId",
                table: "ApiTableNodes",
                column: "ApiDiagramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiEdges");

            migrationBuilder.DropTable(
                name: "ApiTableNodes");

            migrationBuilder.DropTable(
                name: "ApiDiagrams");
        }
    }
}
