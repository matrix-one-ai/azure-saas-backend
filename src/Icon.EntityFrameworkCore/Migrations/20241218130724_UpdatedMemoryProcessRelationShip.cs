using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMemoryProcessRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryProcesses_MemoryProcessId1",
                table: "Memories");

            migrationBuilder.DropIndex(
                name: "IX_Memories_MemoryProcessId1",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryProcessId",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryProcessId1",
                table: "Memories");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemoryId",
                table: "MemoryProcesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemoryProcesses_MemoryId",
                table: "MemoryProcesses",
                column: "MemoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MemoryProcesses_Memories_MemoryId",
                table: "MemoryProcesses",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoryProcesses_Memories_MemoryId",
                table: "MemoryProcesses");

            migrationBuilder.DropIndex(
                name: "IX_MemoryProcesses_MemoryId",
                table: "MemoryProcesses");

            migrationBuilder.AlterColumn<Guid>(
                name: "MemoryId",
                table: "MemoryProcesses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryProcessId",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryProcessId1",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryProcessId1",
                table: "Memories",
                column: "MemoryProcessId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryProcesses_MemoryProcessId1",
                table: "Memories",
                column: "MemoryProcessId1",
                principalTable: "MemoryProcesses",
                principalColumn: "Id");
        }
    }
}
