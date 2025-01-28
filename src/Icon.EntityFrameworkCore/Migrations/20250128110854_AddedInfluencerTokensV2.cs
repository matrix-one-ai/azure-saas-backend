using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedInfluencerTokensV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AvgBuyCountIncrease5m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgBuyersIncrease5m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange15m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange1h",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange30m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange5m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgSellCountDecrease5m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgSellersDecrease5m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange15m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange1h",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange30m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange5m",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgVolumeH1Ratio",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgVolumeM5Ratio",
                table: "InfluencerTokenScores",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgBuyCountIncrease5m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgBuyersIncrease5m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange15m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange1h",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange30m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgMarketCapChange5m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgSellCountDecrease5m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgSellersDecrease5m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange15m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange1h",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange30m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgTotalLiquidityChange5m",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgVolumeH1Ratio",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AvgVolumeM5Ratio",
                table: "InfluencerTokenMentions",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgBuyCountIncrease5m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgBuyersIncrease5m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange15m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange1h",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange30m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange5m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgSellCountDecrease5m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgSellersDecrease5m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange15m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange1h",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange30m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange5m",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgVolumeH1Ratio",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgVolumeM5Ratio",
                table: "InfluencerTokenScores");

            migrationBuilder.DropColumn(
                name: "AvgBuyCountIncrease5m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgBuyersIncrease5m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange15m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange1h",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange30m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgMarketCapChange5m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgSellCountDecrease5m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgSellersDecrease5m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange15m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange1h",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange30m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgTotalLiquidityChange5m",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgVolumeH1Ratio",
                table: "InfluencerTokenMentions");

            migrationBuilder.DropColumn(
                name: "AvgVolumeM5Ratio",
                table: "InfluencerTokenMentions");
        }
    }
}
