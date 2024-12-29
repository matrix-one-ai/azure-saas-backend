using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCharPromptInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PromptInstruction",
                table: "Characters",
                newName: "TwitterMentionReplyInstruction");

            migrationBuilder.RenameColumn(
                name: "OutputExamples",
                table: "Characters",
                newName: "TwitterMentionReplyExamples");

            migrationBuilder.AddColumn<int>(
                name: "TwitterAutoPostDelayMinutes",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TwitterAutoPostExamples",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterAutoPostInstruction",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwitterAutoPostDelayMinutes",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TwitterAutoPostExamples",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TwitterAutoPostInstruction",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "TwitterMentionReplyInstruction",
                table: "Characters",
                newName: "PromptInstruction");

            migrationBuilder.RenameColumn(
                name: "TwitterMentionReplyExamples",
                table: "Characters",
                newName: "OutputExamples");
        }
    }
}
