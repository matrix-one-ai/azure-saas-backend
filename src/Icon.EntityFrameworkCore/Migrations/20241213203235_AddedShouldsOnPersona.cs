using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedShouldsOnPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RespondReplies",
                table: "CharacterPersonas",
                newName: "ShouldRespondNewPosts");

            migrationBuilder.RenameColumn(
                name: "RespondNewPosts",
                table: "CharacterPersonas",
                newName: "ShouldRespondMentions");

            migrationBuilder.AddColumn<bool>(
                name: "ShouldImportNewPosts",
                table: "CharacterPersonas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldImportNewPosts",
                table: "CharacterPersonas");

            migrationBuilder.RenameColumn(
                name: "ShouldRespondNewPosts",
                table: "CharacterPersonas",
                newName: "RespondReplies");

            migrationBuilder.RenameColumn(
                name: "ShouldRespondMentions",
                table: "CharacterPersonas",
                newName: "RespondNewPosts");
        }
    }
}
