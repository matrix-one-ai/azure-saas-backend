using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedRayIndexesV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiscoveryStageName",
                table: "RaydiumPairs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaydiumPair_CombinedMetricScore",
                table: "RaydiumPairs",
                column: "CombinedMetricScore");

            migrationBuilder.CreateIndex(
                name: "IX_RaydiumPair_DiscoveryStageName",
                table: "RaydiumPairs",
                column: "DiscoveryStageName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RaydiumPair_CombinedMetricScore",
                table: "RaydiumPairs");

            migrationBuilder.DropIndex(
                name: "IX_RaydiumPair_DiscoveryStageName",
                table: "RaydiumPairs");

            migrationBuilder.AlterColumn<string>(
                name: "DiscoveryStageName",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
