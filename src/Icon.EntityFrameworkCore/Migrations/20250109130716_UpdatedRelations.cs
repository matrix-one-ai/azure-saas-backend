using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlatformInteractionParentId",
                table: "Memories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlatformInteractionId",
                table: "Memories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memory_PlatformInteractionId",
                table: "Memories",
                column: "PlatformInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Memory_PlatformInteractionParentId",
                table: "Memories",
                column: "PlatformInteractionParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memory_PlatformInteractionId",
                table: "Memories");

            migrationBuilder.DropIndex(
                name: "IX_Memory_PlatformInteractionParentId",
                table: "Memories");

            migrationBuilder.AlterColumn<string>(
                name: "PlatformInteractionParentId",
                table: "Memories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlatformInteractionId",
                table: "Memories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
