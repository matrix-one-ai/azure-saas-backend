using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDbsets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterPersonaTwitterProfile_CharacterPersonas_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterPersonaTwitterProfile",
                table: "CharacterPersonaTwitterProfile");

            migrationBuilder.RenameTable(
                name: "CharacterPersonaTwitterProfile",
                newName: "CharacterPersonaTwitterProfiles");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterPersonaTwitterProfile_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfiles",
                newName: "IX_CharacterPersonaTwitterProfiles_CharacterPersonaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterPersonaTwitterProfiles",
                table: "CharacterPersonaTwitterProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterPersonaTwitterProfiles_CharacterPersonas_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfiles",
                column: "CharacterPersonaId",
                principalTable: "CharacterPersonas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterPersonaTwitterProfiles_CharacterPersonas_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterPersonaTwitterProfiles",
                table: "CharacterPersonaTwitterProfiles");

            migrationBuilder.RenameTable(
                name: "CharacterPersonaTwitterProfiles",
                newName: "CharacterPersonaTwitterProfile");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterPersonaTwitterProfiles_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfile",
                newName: "IX_CharacterPersonaTwitterProfile_CharacterPersonaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterPersonaTwitterProfile",
                table: "CharacterPersonaTwitterProfile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterPersonaTwitterProfile_CharacterPersonas_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfile",
                column: "CharacterPersonaId",
                principalTable: "CharacterPersonas",
                principalColumn: "Id");
        }
    }
}
