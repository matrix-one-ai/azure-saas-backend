using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedCoingeckoPools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPoolUpdate",
                table: "RaydiumPairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoingeckoPoolUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RaydiumPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PoolId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoolType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenPriceUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenPriceNativeCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteTokenPriceUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteTokenPriceNativeCurrency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenPriceQuoteToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteTokenPriceBaseToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoolCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TokenPriceUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FdvUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketCapUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChangeM5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChangeH1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChangeH6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChangeH24 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    M5Buys = table.Column<int>(type: "int", nullable: true),
                    M5Sells = table.Column<int>(type: "int", nullable: true),
                    M5Buyers = table.Column<int>(type: "int", nullable: true),
                    M5Sellers = table.Column<int>(type: "int", nullable: true),
                    M15Buys = table.Column<int>(type: "int", nullable: true),
                    M15Sells = table.Column<int>(type: "int", nullable: true),
                    M15Buyers = table.Column<int>(type: "int", nullable: true),
                    M15Sellers = table.Column<int>(type: "int", nullable: true),
                    M30Buys = table.Column<int>(type: "int", nullable: true),
                    M30Sells = table.Column<int>(type: "int", nullable: true),
                    M30Buyers = table.Column<int>(type: "int", nullable: true),
                    M30Sellers = table.Column<int>(type: "int", nullable: true),
                    H1Buys = table.Column<int>(type: "int", nullable: true),
                    H1Sells = table.Column<int>(type: "int", nullable: true),
                    H1Buyers = table.Column<int>(type: "int", nullable: true),
                    H1Sellers = table.Column<int>(type: "int", nullable: true),
                    H24Buys = table.Column<int>(type: "int", nullable: true),
                    H24Sells = table.Column<int>(type: "int", nullable: true),
                    H24Buyers = table.Column<int>(type: "int", nullable: true),
                    H24Sellers = table.Column<int>(type: "int", nullable: true),
                    VolumeM5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeH1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeH6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolumeH24 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReserveInUsd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteTokenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DexId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoingeckoPoolUpdates", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoingeckoPoolUpdates");

            migrationBuilder.DropColumn(
                name: "LastPoolUpdate",
                table: "RaydiumPairs");
        }
    }
}
