using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedSolWalletForCp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PriceRefreshEnabled",
                table: "RaydiumPairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PriceRefreshIntervalMinutes",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceRefreshLastUpdateTime",
                table: "RaydiumPairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceRefreshNextUpdateTime",
                table: "RaydiumPairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolanaWallet",
                table: "CharacterPersonas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceRefreshEnabled",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "PriceRefreshIntervalMinutes",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "PriceRefreshLastUpdateTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "PriceRefreshNextUpdateTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "SolanaWallet",
                table: "CharacterPersonas");
        }
    }
}
