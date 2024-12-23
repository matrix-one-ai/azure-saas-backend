using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwitterEnabledOnCharv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPromptingEnabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTwitterPostingEnabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTwitterScrapingEnabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TwitterPostAgentId",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterScrapeAgentId",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPromptingEnabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IsTwitterPostingEnabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "IsTwitterScrapingEnabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TwitterPostAgentId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TwitterScrapeAgentId",
                table: "Characters");
        }
    }
}
