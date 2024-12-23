using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedPromptsAndActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromptForAction",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "PromptResponse",
                table: "Memories");

            migrationBuilder.AddColumn<bool>(
                name: "IsActionTaken",
                table: "Memories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromptGenerated",
                table: "Memories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MemoryActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    MemoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemoryPromptId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryActions_Memories_MemoryId",
                        column: x => x.MemoryId,
                        principalTable: "Memories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemoryPrompts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PromptType = table.Column<int>(type: "int", nullable: false),
                    MemoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InputContextModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputContextJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputFullText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryPrompts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryPrompts_Memories_MemoryId",
                        column: x => x.MemoryId,
                        principalTable: "Memories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemoryActions_MemoryId",
                table: "MemoryActions",
                column: "MemoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryPrompts_MemoryId",
                table: "MemoryPrompts",
                column: "MemoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoryActions");

            migrationBuilder.DropTable(
                name: "MemoryPrompts");

            migrationBuilder.DropColumn(
                name: "IsActionTaken",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "IsPromptGenerated",
                table: "Memories");

            migrationBuilder.AddColumn<string>(
                name: "PromptForAction",
                table: "Memories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromptResponse",
                table: "Memories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
