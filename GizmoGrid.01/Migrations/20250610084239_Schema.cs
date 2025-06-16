using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchemaDiagrams",
                columns: table => new
                {
                    SchemaDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemaDiagrams", x => x.SchemaDiagramId);
                    table.ForeignKey(
                        name: "FK_SchemaDiagrams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableNodes",
                columns: table => new
                {
                    TableNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemaDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositionX = table.Column<float>(type: "real", nullable: false),
                    PositionY = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableNodes", x => x.TableNodeId);
                    table.ForeignKey(
                        name: "FK_TableNodes_SchemaDiagrams_SchemaDiagramId",
                        column: x => x.SchemaDiagramId,
                        principalTable: "SchemaDiagrams",
                        principalColumn: "SchemaDiagramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableColumns",
                columns: table => new
                {
                    TableColumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableNodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimaryKey = table.Column<bool>(type: "bit", nullable: false),
                    IsForeignKey = table.Column<bool>(type: "bit", nullable: false),
                    IsNullable = table.Column<bool>(type: "bit", nullable: false),
                    IsUnique = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableColumns", x => x.TableColumnId);
                    table.ForeignKey(
                        name: "FK_TableColumns_TableNodes_TableNodeId",
                        column: x => x.TableNodeId,
                        principalTable: "TableNodes",
                        principalColumn: "TableNodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableEdges",
                columns: table => new
                {
                    EdgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchemaDiagramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableEdges", x => x.EdgeId);
                    table.ForeignKey(
                        name: "FK_TableEdges_SchemaDiagrams_SchemaDiagramId",
                        column: x => x.SchemaDiagramId,
                        principalTable: "SchemaDiagrams",
                        principalColumn: "SchemaDiagramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableEdges_TableNodes_SourceId",
                        column: x => x.SourceId,
                        principalTable: "TableNodes",
                        principalColumn: "TableNodeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TableEdges_TableNodes_TargetId",
                        column: x => x.TargetId,
                        principalTable: "TableNodes",
                        principalColumn: "TableNodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchemaDiagrams_UserId",
                table: "SchemaDiagrams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TableColumns_TableNodeId",
                table: "TableColumns",
                column: "TableNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_TableEdges_SchemaDiagramId",
                table: "TableEdges",
                column: "SchemaDiagramId");

            migrationBuilder.CreateIndex(
                name: "IX_TableEdges_SourceId",
                table: "TableEdges",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TableEdges_TargetId",
                table: "TableEdges",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_TableNodes_SchemaDiagramId",
                table: "TableNodes",
                column: "SchemaDiagramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableColumns");

            migrationBuilder.DropTable(
                name: "TableEdges");

            migrationBuilder.DropTable(
                name: "TableNodes");

            migrationBuilder.DropTable(
                name: "SchemaDiagrams");
        }
    }
}
