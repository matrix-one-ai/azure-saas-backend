using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTwitterRanking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalTweets",
                table: "CharacterPersonaTwitterRanks",
                newName: "TotalSentimentScore");

            migrationBuilder.RenameColumn(
                name: "PersonaScore",
                table: "CharacterPersonaTwitterRanks",
                newName: "TotalScoreTimeDecayed");

            migrationBuilder.AddColumn<int>(
                name: "DepthScore",
                table: "MemoryStatsTwitters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoveltyScore",
                table: "MemoryStatsTwitters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RelevanceScore",
                table: "MemoryStatsTwitters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SentimentScore",
                table: "MemoryStatsTwitters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalDepthScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalEngagementScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalMentions",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalMentionsScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalNoveltyScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalQualityScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalRelevanceScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "CharacterPersonaTwitterRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepthScore",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropColumn(
                name: "NoveltyScore",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropColumn(
                name: "RelevanceScore",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropColumn(
                name: "SentimentScore",
                table: "MemoryStatsTwitters");

            migrationBuilder.DropColumn(
                name: "TotalDepthScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalEngagementScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalMentions",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalMentionsScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalNoveltyScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalQualityScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalRelevanceScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.RenameColumn(
                name: "TotalSentimentScore",
                table: "CharacterPersonaTwitterRanks",
                newName: "TotalTweets");

            migrationBuilder.RenameColumn(
                name: "TotalScoreTimeDecayed",
                table: "CharacterPersonaTwitterRanks",
                newName: "PersonaScore");
        }
    }
}
