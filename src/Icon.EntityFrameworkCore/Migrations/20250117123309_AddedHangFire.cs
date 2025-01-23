using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedHangFire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceRefreshIntervalMinutes",
                table: "RaydiumPairs",
                newName: "PriceRefreshIntervalSeconds");

            migrationBuilder.AddColumn<bool>(
                name: "TweetSent",
                table: "RaydiumPairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TweetSentAt",
                table: "RaydiumPairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaydiumPair_CreationTime",
                table: "RaydiumPairs",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_RaydiumPair_PriceRefreshEnabled",
                table: "RaydiumPairs",
                column: "PriceRefreshEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_RaydiumPair_Token0",
                table: "RaydiumPairs",
                column: "PriceRefreshNextUpdateTime");

            migrationBuilder.CreateIndex(
                name: "IX_RaydiumPair_TweetSent",
                table: "RaydiumPairs",
                column: "TweetSent");

            migrationBuilder.CreateIndex(
                name: "IX_CoingeckoPoolUpdate_CreationTime",
                table: "CoingeckoPoolUpdates",
                column: "CreationTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RaydiumPair_CreationTime",
                table: "RaydiumPairs");

            migrationBuilder.DropIndex(
                name: "IX_RaydiumPair_PriceRefreshEnabled",
                table: "RaydiumPairs");

            migrationBuilder.DropIndex(
                name: "IX_RaydiumPair_Token0",
                table: "RaydiumPairs");

            migrationBuilder.DropIndex(
                name: "IX_RaydiumPair_TweetSent",
                table: "RaydiumPairs");

            migrationBuilder.DropIndex(
                name: "IX_CoingeckoPoolUpdate_CreationTime",
                table: "CoingeckoPoolUpdates");

            migrationBuilder.DropColumn(
                name: "TweetSent",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetSentAt",
                table: "RaydiumPairs");

            migrationBuilder.RenameColumn(
                name: "PriceRefreshIntervalSeconds",
                table: "RaydiumPairs",
                newName: "PriceRefreshIntervalMinutes");
        }
    }
}
