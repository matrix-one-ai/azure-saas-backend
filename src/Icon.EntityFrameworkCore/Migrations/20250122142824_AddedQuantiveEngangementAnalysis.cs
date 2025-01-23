using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedQuantiveEngangementAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EngagementCorrelationAnalysis",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "EngagementCorrelationScore",
                table: "RaydiumPairs",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlantFinalPrediction",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlantFinalSummary",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngagementCorrelationAnalysis",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "EngagementCorrelationScore",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "PlantFinalPrediction",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "PlantFinalSummary",
                table: "RaydiumPairs");
        }
    }
}
