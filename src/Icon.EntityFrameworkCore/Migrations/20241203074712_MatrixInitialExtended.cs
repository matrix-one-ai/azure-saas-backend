using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class MatrixInitialExtended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryType_MemoryTypeId",
                table: "Memories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoryType",
                table: "MemoryType");

            migrationBuilder.RenameTable(
                name: "MemoryType",
                newName: "MemoryTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoryTypes",
                table: "MemoryTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PersonaPlatforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    PersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformPersonaId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonaPlatforms_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonaPlatforms_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonaPlatforms_PersonaId",
                table: "PersonaPlatforms",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaPlatforms_PlatformId",
                table: "PersonaPlatforms",
                column: "PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryTypes_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId",
                principalTable: "MemoryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryTypes_MemoryTypeId",
                table: "Memories");

            migrationBuilder.DropTable(
                name: "PersonaPlatforms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MemoryTypes",
                table: "MemoryTypes");

            migrationBuilder.RenameTable(
                name: "MemoryTypes",
                newName: "MemoryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemoryType",
                table: "MemoryType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryType_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId",
                principalTable: "MemoryType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
