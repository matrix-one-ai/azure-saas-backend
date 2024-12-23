using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharacterBioRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId",
                table: "CharacterBios");

            migrationBuilder.DropForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId1",
                table: "CharacterBios");

            migrationBuilder.DropIndex(
                name: "IX_CharacterBios_CharacterId1",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "CharacterId1",
                table: "CharacterBios");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId",
                table: "CharacterBios",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId",
                table: "CharacterBios");

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterId1",
                table: "CharacterBios",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterBios_CharacterId1",
                table: "CharacterBios",
                column: "CharacterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId",
                table: "CharacterBios",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId1",
                table: "CharacterBios",
                column: "CharacterId1",
                principalTable: "Characters",
                principalColumn: "Id");
        }
    }
}
