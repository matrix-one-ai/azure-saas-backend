using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedMemStartAsDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryStatsTwitter_MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.DropForeignKey(
                name: "FK_MemoryStatsTwitter_Memories_MemoryId1",
                table: "MemoryStatsTwitter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoryStatsTwitter",
                table: "MemoryStatsTwitter");

            migrationBuilder.RenameTable(
                name: "MemoryStatsTwitter",
                newName: "MemoryStatsTwitters");

            migrationBuilder.RenameIndex(
                name: "IX_MemoryStatsTwitter_MemoryId1",
                table: "MemoryStatsTwitters",
                newName: "IX_MemoryStatsTwitters_MemoryId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoryStatsTwitters",
                table: "MemoryStatsTwitters",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryStatsTwitters_MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.DropForeignKey(
                name: "FK_MemoryStatsTwitters_Memories_MemoryId1",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoryStatsTwitters",
                table: "MemoryStatsTwitters");

            migrationBuilder.RenameTable(
                name: "MemoryStatsTwitters",
                newName: "MemoryStatsTwitter");

            migrationBuilder.RenameIndex(
                name: "IX_MemoryStatsTwitters_MemoryId1",
                table: "MemoryStatsTwitter",
                newName: "IX_MemoryStatsTwitter_MemoryId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoryStatsTwitter",
                table: "MemoryStatsTwitter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryStatsTwitter_MemoryStatsTwitterId",
                table: "Memories",
                column: "MemoryStatsTwitterId",
                principalTable: "MemoryStatsTwitter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemoryStatsTwitter_Memories_MemoryId1",
                table: "MemoryStatsTwitter",
                column: "MemoryId1",
                principalTable: "Memories",
                principalColumn: "Id");
        }
    }
}
