using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedDummyBuySell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DummyBuyAmount",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DummyBuyPrice",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DummyBuyQuantity",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DummyBuyTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DummySellAmount",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DummySellPrice",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DummySellQuantity",
                table: "RaydiumPairs",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DummySellTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EngagementBuyRecommendation",
                table: "RaydiumPairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EngagementRecommendedSellByTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenHolderData",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DummyBuyAmount",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummyBuyPrice",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummyBuyQuantity",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummyBuyTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummySellAmount",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummySellPrice",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummySellQuantity",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "DummySellTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "EngagementBuyRecommendation",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "EngagementRecommendedSellByTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TokenHolderData",
                table: "RaydiumPairs");
        }
    }
}
