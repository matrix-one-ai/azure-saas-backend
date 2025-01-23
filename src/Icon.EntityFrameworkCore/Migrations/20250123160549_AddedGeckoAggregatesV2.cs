using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedGeckoAggregatesV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoingeckoAggregatedUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Pools = table.Column<int>(type: "int", nullable: false),
                    TotalLiquidityUsd = table.Column<float>(type: "real", nullable: false),
                    WeightedAvgPriceUsd = table.Column<float>(type: "real", nullable: false),
                    FdvUsd = table.Column<float>(type: "real", nullable: false),
                    MarketCapUsd = table.Column<float>(type: "real", nullable: false),
                    PriceChangeM5 = table.Column<float>(type: "real", nullable: false),
                    PriceChangeH1 = table.Column<float>(type: "real", nullable: false),
                    PriceChangeH6 = table.Column<float>(type: "real", nullable: false),
                    PriceChangeH24 = table.Column<float>(type: "real", nullable: false),
                    VolumeM5 = table.Column<float>(type: "real", nullable: false),
                    VolumeH1 = table.Column<float>(type: "real", nullable: false),
                    VolumeH6 = table.Column<float>(type: "real", nullable: false),
                    VolumeH24 = table.Column<float>(type: "real", nullable: false),
                    M5Buys = table.Column<int>(type: "int", nullable: false),
                    M5Sells = table.Column<int>(type: "int", nullable: false),
                    M15Buys = table.Column<int>(type: "int", nullable: false),
                    M15Sells = table.Column<int>(type: "int", nullable: false),
                    M30Buys = table.Column<int>(type: "int", nullable: false),
                    M30Sells = table.Column<int>(type: "int", nullable: false),
                    H1Buys = table.Column<int>(type: "int", nullable: false),
                    H1Sells = table.Column<int>(type: "int", nullable: false),
                    H24Buys = table.Column<int>(type: "int", nullable: false),
                    H24Sells = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoingeckoAggregatedUpdates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoingeckoAggregatedUpdates");
        }
    }
}
