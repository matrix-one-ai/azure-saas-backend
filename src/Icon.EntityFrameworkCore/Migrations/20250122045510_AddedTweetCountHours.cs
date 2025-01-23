using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTweetCountHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TweetsCATweetCount12H",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCATweetCount1H",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCATweetCount24H",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCATweetCount3H",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TweetsCATweetCount6H",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TweetsCATweetCount12H",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCATweetCount1H",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCATweetCount24H",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCATweetCount3H",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TweetsCATweetCount6H",
                table: "RaydiumPairs");
        }
    }
}
