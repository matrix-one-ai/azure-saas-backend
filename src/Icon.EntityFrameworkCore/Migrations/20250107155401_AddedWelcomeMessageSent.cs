using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedWelcomeMessageSent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WelcomeMessageSent",
                table: "CharacterPersonas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "WelcomeMessageSentAt",
                table: "CharacterPersonas",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WelcomeMessageSent",
                table: "CharacterPersonas");

            migrationBuilder.DropColumn(
                name: "WelcomeMessageSentAt",
                table: "CharacterPersonas");
        }
    }
}
