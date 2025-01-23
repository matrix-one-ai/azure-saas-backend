using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuantivePairAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "BuySellRatioQualitativeAnalysis",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuySellRatioRecommendation",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "BuySellRatioValue",
                table: "RaydiumPairs",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CombinedMetricScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CombinedQualitativeAnalysis",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CombinedRecommendation",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DiscoveryStageLastUpdated",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscoveryStageName",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "LiquidityUsd",
                table: "RaydiumPairs",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VlrQualitativeAnalysis",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VlrRecommendation",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "VlrValue",
                table: "RaydiumPairs",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VolumeToFdv1HQualitativeAnalysis",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VolumeToFdv1HRecommendation",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "VolumeToFdv1HValue",
                table: "RaydiumPairs",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuySellRatioQualitativeAnalysis",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "BuySellRatioRecommendation",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "BuySellRatioValue",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "CombinedMetricScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "CombinedQualitativeAnalysis",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "CombinedRecommendation",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DiscoveryStageLastUpdated",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DiscoveryStageName",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "LiquidityUsd",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "VlrQualitativeAnalysis",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "VlrRecommendation",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "VlrValue",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "VolumeToFdv1HQualitativeAnalysis",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "VolumeToFdv1HRecommendation",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "VolumeToFdv1HValue",
                table: "RaydiumPairs");

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
    }
}
