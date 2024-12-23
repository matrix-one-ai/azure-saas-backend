using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UPdatedMemStatsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryStatsTwitters_MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.DropForeignKey(
                name: "FK_MemoryStatsTwitters_Memories_MemoryId1",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropIndex(
                name: "IX_MemoryStatsTwitters_MemoryId1",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropIndex(
                name: "IX_Memories_MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryId1",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropColumn(
                name: "MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryStatsTwitters_MemoryId",
                table: "MemoryStatsTwitters",
                column: "MemoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MemoryStatsTwitters_Memories_MemoryId",
                table: "MemoryStatsTwitters",
                column: "MemoryId",
                principalTable: "Memories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemoryStatsTwitters_Memories_MemoryId",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropIndex(
                name: "IX_MemoryStatsTwitters_MemoryId",
                table: "MemoryStatsTwitters");

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryId1",
                table: "MemoryStatsTwitters",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryStatsTwitterId",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemoryStatsTwitters_MemoryId1",
                table: "MemoryStatsTwitters",
                column: "MemoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryStatsTwitterId",
                table: "Memories",
                column: "MemoryStatsTwitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryStatsTwitters_MemoryStatsTwitterId",
                table: "Memories",
                column: "MemoryStatsTwitterId",
                principalTable: "MemoryStatsTwitters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemoryStatsTwitters_Memories_MemoryId1",
                table: "MemoryStatsTwitters",
                column: "MemoryId1",
                principalTable: "Memories",
                principalColumn: "Id");
        }
    }
}
