using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedInfluencerTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfluencerTokenMentions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Influencer = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RaydiumPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    AvgPump5m = table.Column<float>(type: "real", nullable: true),
                    AvgPump15m = table.Column<float>(type: "real", nullable: true),
                    AvgPump30m = table.Column<float>(type: "real", nullable: true),
                    AvgPump1h = table.Column<float>(type: "real", nullable: true),
                    AvgPump2h = table.Column<float>(type: "real", nullable: true),
                    SuccessRate5m = table.Column<float>(type: "real", nullable: true),
                    SuccessRate15m = table.Column<float>(type: "real", nullable: true),
                    SuccessRate30m = table.Column<float>(type: "real", nullable: true),
                    SuccessRate1h = table.Column<float>(type: "real", nullable: true),
                    SuccessRate2h = table.Column<float>(type: "real", nullable: true),
                    DeathCount = table.Column<int>(type: "int", nullable: false),
                    AliveCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerTokenMentions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InfluencerTokenScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Influencer = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    UniqueTokensCount = table.Column<int>(type: "int", nullable: false),
                    AvgPump5m = table.Column<float>(type: "real", nullable: true),
                    AvgPump15m = table.Column<float>(type: "real", nullable: true),
                    AvgPump30m = table.Column<float>(type: "real", nullable: true),
                    AvgPump1h = table.Column<float>(type: "real", nullable: true),
                    AvgPump2h = table.Column<float>(type: "real", nullable: true),
                    SuccessRate5m = table.Column<float>(type: "real", nullable: true),
                    SuccessRate15m = table.Column<float>(type: "real", nullable: true),
                    SuccessRate30m = table.Column<float>(type: "real", nullable: true),
                    SuccessRate1h = table.Column<float>(type: "real", nullable: true),
                    SuccessRate2h = table.Column<float>(type: "real", nullable: true),
                    DeathCount = table.Column<int>(type: "int", nullable: false),
                    AliveCount = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<float>(type: "real", nullable: true),
                    TimedWindowScore = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerTokenScores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerTokenMention_Influencer",
                table: "InfluencerTokenMentions",
                column: "Influencer");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerTokenMention_RaydiumPairId",
                table: "InfluencerTokenMentions",
                column: "RaydiumPairId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerTokenScore_Influencer",
                table: "InfluencerTokenScores",
                column: "Influencer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfluencerTokenMentions");

            migrationBuilder.DropTable(
                name: "InfluencerTokenScores");
        }
    }
}
