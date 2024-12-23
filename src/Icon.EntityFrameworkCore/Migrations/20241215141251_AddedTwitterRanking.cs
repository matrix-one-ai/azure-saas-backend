using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwitterRanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterPersonaTwitterRanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterPersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalTweets = table.Column<int>(type: "int", nullable: false),
                    TotalLikes = table.Column<int>(type: "int", nullable: false),
                    TotalReplies = table.Column<int>(type: "int", nullable: false),
                    TotalRetweets = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false),
                    TotalMentionsCount = table.Column<int>(type: "int", nullable: false),
                    TotalWordCount = table.Column<int>(type: "int", nullable: false),
                    PersonaScore = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPersonaTwitterRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterPersonaTwitterRanks_CharacterPersonas_CharacterPersonaId",
                        column: x => x.CharacterPersonaId,
                        principalTable: "CharacterPersonas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPersonaTwitterRanks_CharacterPersonaId",
                table: "CharacterPersonaTwitterRanks",
                column: "CharacterPersonaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterPersonaTwitterRanks");
        }
    }
}
