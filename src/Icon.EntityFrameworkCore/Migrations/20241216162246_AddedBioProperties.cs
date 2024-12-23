using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedBioProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterPersonaTwitterRanks_CharacterPersonaId",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.AddColumn<string>(
                name: "Backstory",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BirthDate",
                table: "CharacterBios",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CrisisBehavior",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fears",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaPresence",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivations",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivateSelf",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicPersona",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Relationships",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpeechPatterns",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechDetails",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "CharacterBios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPersonaTwitterRanks_CharacterPersonaId",
                table: "CharacterPersonaTwitterRanks",
                column: "CharacterPersonaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CharacterPersonaTwitterRanks_CharacterPersonaId",
                table: "CharacterPersonaTwitterRanks");

            migrationBuilder.DropColumn(
                name: "Backstory",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "CrisisBehavior",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "Fears",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "MediaPresence",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "Motivations",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "PrivateSelf",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "PublicPersona",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "Relationships",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "SpeechPatterns",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "TechDetails",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "CharacterBios");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPersonaTwitterRanks_CharacterPersonaId",
                table: "CharacterPersonaTwitterRanks",
                column: "CharacterPersonaId");
        }
    }
}
