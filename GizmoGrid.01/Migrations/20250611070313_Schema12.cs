using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GizmoGrid._01.Migrations
{
    /// <inheritdoc />
    public partial class Schema12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForeignKey",
                table: "TableColumns");

            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "TableColumns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ForeignKeyReferenceColumnId",
                table: "TableColumns",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TableColumns_ForeignKeyReferenceColumnId",
                table: "TableColumns",
                column: "ForeignKeyReferenceColumnId");

            migrationBuilder.AddForeignKey(
                name: "FK_TableColumns_TableColumns_ForeignKeyReferenceColumnId",
                table: "TableColumns",
                column: "ForeignKeyReferenceColumnId",
                principalTable: "TableColumns",
                principalColumn: "TableColumnId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TableColumns_TableColumns_ForeignKeyReferenceColumnId",
                table: "TableColumns");

            migrationBuilder.DropIndex(
                name: "IX_TableColumns_ForeignKeyReferenceColumnId",
                table: "TableColumns");

            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "TableColumns");

            migrationBuilder.DropColumn(
                name: "ForeignKeyReferenceColumnId",
                table: "TableColumns");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKey",
                table: "TableColumns",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
