using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class MatrixInitialFixedv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_Personas_CharacterPersonaId",
                table: "Memories");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_CharacterPersonas_CharacterPersonaId",
                table: "Memories",
                column: "CharacterPersonaId",
                principalTable: "CharacterPersonas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_CharacterPersonas_CharacterPersonaId",
                table: "Memories");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_Personas_CharacterPersonaId",
                table: "Memories",
                column: "CharacterPersonaId",
                principalTable: "Personas",
                principalColumn: "Id");
        }
    }
}
