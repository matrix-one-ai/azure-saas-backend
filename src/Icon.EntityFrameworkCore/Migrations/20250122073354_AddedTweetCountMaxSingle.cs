using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTweetCountMaxSingle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TwitterCAMostLikesOnSingleTweet",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwitterCAMostRepliesOnSingleTweet",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwitterCAMostLikesOnSingleTweet",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCAMostRepliesOnSingleTweet",
                table: "RaydiumPairs");
        }
    }
}
