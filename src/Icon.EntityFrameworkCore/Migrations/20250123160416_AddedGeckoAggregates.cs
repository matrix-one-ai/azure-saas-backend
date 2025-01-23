using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedGeckoAggregates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPoolUpdate",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TokenCombinedScore",
                table: "RaydiumPairs");

            migrationBuilder.RenameColumn(
                name: "LiquidityUsd",
                table: "RaydiumPairs",
                newName: "TotalLiquidityUsd");

            migrationBuilder.AddColumn<Guid>(
                name: "CoingeckoLastAggregatedUpdateId",
                table: "RaydiumPairs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CoingeckoAggregatedUpdateId",
                table: "CoingeckoPoolUpdates",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoingeckoLastAggregatedUpdateId",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "CoingeckoAggregatedUpdateId",
                table: "CoingeckoPoolUpdates");

            migrationBuilder.RenameColumn(
                name: "TotalLiquidityUsd",
                table: "RaydiumPairs",
                newName: "LiquidityUsd");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastPoolUpdate",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TokenCombinedScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
