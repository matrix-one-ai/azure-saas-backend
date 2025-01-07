using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedRaydiumPairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaydiumPairs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Slot = table.Column<long>(type: "bigint", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlockTime = table.Column<long>(type: "bigint", nullable: false),
                    SourceExchange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmmAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenDecimals = table.Column<int>(type: "int", nullable: true),
                    BaseTokenSupply = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenSymbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseTokenLiquidityAdded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteTokenAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuoteTokenLiquidityAdded = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaydiumPairs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaydiumPairs");
        }
    }
}
