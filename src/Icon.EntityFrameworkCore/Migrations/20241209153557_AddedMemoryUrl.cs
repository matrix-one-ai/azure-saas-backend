using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedMemoryUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemoryUrl",
                table: "Memories",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "FK_CharacterBios_Characters_CharacterId1",
                table: "CharacterBios",
                column: "CharacterId1",
                principalTable: "Characters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterBios_Characters_CharacterId1",
                table: "CharacterBios");

            migrationBuilder.DropIndex(
                name: "IX_CharacterBios_CharacterId1",
                table: "CharacterBios");

            migrationBuilder.DropColumn(
                name: "MemoryUrl",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "CharacterId1",
                table: "CharacterBios");
        }
    }
}
