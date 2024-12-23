using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwitterStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryTypes_MemoryTypeId",
                table: "Memories");

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryStatsTwitterId",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemoryStatsTwitter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MemoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemoryId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPin = table.Column<bool>(type: "bit", nullable: false),
                    IsQuoted = table.Column<bool>(type: "bit", nullable: false),
                    IsReply = table.Column<bool>(type: "bit", nullable: false),
                    IsRetweet = table.Column<bool>(type: "bit", nullable: false),
                    SensitiveContent = table.Column<bool>(type: "bit", nullable: false),
                    BookmarkCount = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Replies = table.Column<int>(type: "int", nullable: false),
                    Retweets = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    TweetWordCount = table.Column<int>(type: "int", nullable: false),
                    MentionsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryStatsTwitter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryStatsTwitter_Memories_MemoryId1",
                        column: x => x.MemoryId1,
                        principalTable: "Memories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryStatsTwitterId",
                table: "Memories",
                column: "MemoryStatsTwitterId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryStatsTwitter_MemoryId1",
                table: "MemoryStatsTwitter",
                column: "MemoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryStatsTwitter_MemoryStatsTwitterId",
                table: "Memories",
                column: "MemoryStatsTwitterId",
                principalTable: "MemoryStatsTwitter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryTypes_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId",
                principalTable: "MemoryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryStatsTwitter_MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryTypes_MemoryTypeId",
                table: "Memories");

            migrationBuilder.DropTable(
                name: "MemoryStatsTwitter");

            migrationBuilder.DropIndex(
                name: "IX_Memories_MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryStatsTwitterId",
                table: "Memories");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryTypes_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId",
                principalTable: "MemoryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
