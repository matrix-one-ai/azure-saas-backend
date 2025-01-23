using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTokenEngagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaydiumPairTwitterEngagements");

            migrationBuilder.RenameColumn(
                name: "TwitterCALookupCount",
                table: "RaydiumPairs",
                newName: "TwitterCAQueryCount");

            migrationBuilder.RenameColumn(
                name: "TwitterCAFirstMention",
                table: "RaydiumPairs",
                newName: "TwitterCAFirstMentionTime");

            migrationBuilder.AddColumn<float>(
                name: "TokenCombinedScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementTotalLikes",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementTotalQuotes",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementTotalReplies",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementTotalRetweets",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementTotalViews",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementTweetsImported",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCATweetCount",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TwitterCAFirstMentionHandle",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TwitterAPIUsages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Query = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    RateLimitType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RateLimitRemaining = table.Column<int>(type: "int", nullable: true),
                    RateLimitLimit = table.Column<int>(type: "int", nullable: true),
                    RateLimitResetTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAPIUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterImportTweetCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaydiumPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SearchQuery = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TweetCount = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterImportTweetCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterImportTweetEngagements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaydiumPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TweetId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    ReplyCount = table.Column<int>(type: "int", nullable: false),
                    RetweetCount = table.Column<int>(type: "int", nullable: false),
                    QuoteCount = table.Column<int>(type: "int", nullable: false),
                    ImpressionCount = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterImportTweetEngagements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoingeckoPoolUpdate_RaydiumPair_CreationTime",
                table: "CoingeckoPoolUpdates",
                columns: new[] { "RaydiumPairId", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweetCount_CreationTime",
                table: "TwitterImportTweetCounts",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweetCount_RaydiumPairId",
                table: "TwitterImportTweetCounts",
                column: "RaydiumPairId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweetCount_StartEndTimePairQuery",
                table: "TwitterImportTweetCounts",
                columns: new[] { "StartTime", "EndTime", "RaydiumPairId", "SearchQuery" });

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweetEngagement_CreatedAt",
                table: "TwitterImportTweetEngagements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweetEngagement_RaydiumPairId",
                table: "TwitterImportTweetEngagements",
                column: "RaydiumPairId");

            migrationBuilder.CreateIndex(
                name: "UX_TwitterImportTweetEngagement_RaydiumPair_TweetId",
                table: "TwitterImportTweetEngagements",
                columns: new[] { "RaydiumPairId", "TweetId" },
                unique: true,
                filter: "[TweetId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitterAPIUsages");

            migrationBuilder.DropTable(
                name: "TwitterImportTweetCounts");

            migrationBuilder.DropTable(
                name: "TwitterImportTweetEngagements");

            migrationBuilder.DropIndex(
                name: "IX_CoingeckoPoolUpdate_RaydiumPair_CreationTime",
                table: "CoingeckoPoolUpdates");

            migrationBuilder.DropColumn(
                name: "TokenCombinedScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementTotalLikes",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementTotalQuotes",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementTotalReplies",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementTotalRetweets",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementTotalViews",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementTweetsImported",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCATweetCount",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCAFirstMentionHandle",
                table: "RaydiumPairs");

            migrationBuilder.RenameColumn(
                name: "TwitterCAQueryCount",
                table: "RaydiumPairs",
                newName: "TwitterCALookupCount");

            migrationBuilder.RenameColumn(
                name: "TwitterCAFirstMentionTime",
                table: "RaydiumPairs",
                newName: "TwitterCAFirstMention");

            migrationBuilder.CreateTable(
                name: "RaydiumPairTwitterEngagements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RaydiumPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaydiumPairTwitterEngagements", x => x.Id);
                });
        }
    }
}
