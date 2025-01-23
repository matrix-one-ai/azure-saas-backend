using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRayPairEngagment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "TokenLikeNormScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TokenLiquidityNormScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TokenPriceChange24NormScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TokenRetweetNormScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TokenTweetCountNormScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenLikeNormScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TokenLiquidityNormScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TokenPriceChange24NormScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TokenRetweetNormScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TokenTweetCountNormScore",
                table: "RaydiumPairs");
        }
    }
}
