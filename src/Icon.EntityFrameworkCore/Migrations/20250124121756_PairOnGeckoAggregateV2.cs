using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class PairOnGeckoAggregateV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "H1Buyers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "H1Sellers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "H24Buyers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "H24Sellers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "M15Buyers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "M15Sellers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "M30Buyers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "M30Sellers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "M5Buyers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "M5Sellers",
                table: "CoingeckoAggregatedUpdates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "H1Buyers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "H1Sellers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "H24Buyers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "H24Sellers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "M15Buyers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "M15Sellers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "M30Buyers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "M30Sellers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "M5Buyers",
                table: "CoingeckoAggregatedUpdates");

            migrationBuilder.DropColumn(
                name: "M5Sellers",
                table: "CoingeckoAggregatedUpdates");
        }
    }
}
