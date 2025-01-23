using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedRaydiumTweetText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TweetText",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoingeckoPoolUpdate_RaydiumPairId",
                table: "CoingeckoPoolUpdates",
                column: "RaydiumPairId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CoingeckoPoolUpdate_RaydiumPairId",
                table: "CoingeckoPoolUpdates");

            migrationBuilder.DropColumn(
                name: "TweetText",
                table: "RaydiumPairs");
        }
    }
}
