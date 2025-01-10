using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedConvoPersonaLimitsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MemoryParentId",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemoryParents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PlatformInteractionParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MemoryCount = table.Column<int>(type: "int", nullable: false),
                    CharacterReplyCount = table.Column<int>(type: "int", nullable: false),
                    UniquePersonasCount = table.Column<int>(type: "int", nullable: false),
                    LastReplyAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryParents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryParentId",
                table: "Memories",
                column: "MemoryParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Memory_CreatedAt",
                table: "Memories",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryParent_PlatformInteractionParentId",
                table: "MemoryParents",
                column: "PlatformInteractionParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryParents_MemoryParentId",
                table: "Memories",
                column: "MemoryParentId",
                principalTable: "MemoryParents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryParents_MemoryParentId",
                table: "Memories");

            migrationBuilder.DropTable(
                name: "MemoryParents");

            migrationBuilder.DropIndex(
                name: "IX_Memories_MemoryParentId",
                table: "Memories");

            migrationBuilder.DropIndex(
                name: "IX_Memory_CreatedAt",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryParentId",
                table: "Memories");
        }
    }
}
