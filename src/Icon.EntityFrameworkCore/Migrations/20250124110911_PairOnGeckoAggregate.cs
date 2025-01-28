using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class PairOnGeckoAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TweetId",
                table: "TwitterImportTweets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCAEngagementUniqueAuthors",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RaydiumPairId",
                table: "CoingeckoAggregatedUpdates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweet_CharacterId",
                table: "TwitterImportTweets",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterImportTweet_TweetId",
                table: "TwitterImportTweets",
                column: "TweetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TwitterImportTweet_CharacterId",
                table: "TwitterImportTweets");

            migrationBuilder.DropIndex(
                name: "IX_TwitterImportTweet_TweetId",
                table: "TwitterImportTweets");

            migrationBuilder.DropColumn(
                name: "TweetsCAEngagementUniqueAuthors",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "RaydiumPairId",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.AlterColumn<string>(
                name: "TweetId",
                table: "TwitterImportTweets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
