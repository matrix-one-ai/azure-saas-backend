using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class MatrixInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemoryType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterBios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Personality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Appearance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveFrom = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ActiveTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterBios_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPersonas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attitude = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Repsonses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RespondNewPosts = table.Column<bool>(type: "bit", nullable: false),
                    RespondReplies = table.Column<bool>(type: "bit", nullable: false),
                    PersonaIsAi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPersonas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterPersonas_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterPersonas_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPlatforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformCharacterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformCharacterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformLogin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPlatforms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterPlatforms_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterPlatforms_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Attitudes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Repsonses = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterTopics_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterTopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterBioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlatformInteractionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformInteractionParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlatformInteractionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CharacterPersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MemoryTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemoryTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemoryContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RememberDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromptForAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromptResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShouldVectorize = table.Column<bool>(type: "bit", nullable: false),
                    VectorHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memories_CharacterBios_CharacterBioId",
                        column: x => x.CharacterBioId,
                        principalTable: "CharacterBios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memories_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Memories_MemoryType_MemoryTypeId",
                        column: x => x.MemoryTypeId,
                        principalTable: "MemoryType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memories_Personas_CharacterPersonaId",
                        column: x => x.CharacterPersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Memories_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemoryTopics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryTopics_Memories_MemoryId",
                        column: x => x.MemoryId,
                        principalTable: "Memories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemoryTopics_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterBios_CharacterId",
                table: "CharacterBios",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPersonas_CharacterId",
                table: "CharacterPersonas",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPersonas_PersonaId",
                table: "CharacterPersonas",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPlatforms_CharacterId",
                table: "CharacterPlatforms",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPlatforms_PlatformId",
                table: "CharacterPlatforms",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTopics_CharacterId",
                table: "CharacterTopics",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTopics_TopicId",
                table: "CharacterTopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_CharacterBioId",
                table: "Memories",
                column: "CharacterBioId");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_CharacterId",
                table: "Memories",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_CharacterPersonaId",
                table: "Memories",
                column: "CharacterPersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryTypeId",
                table: "Memories",
                column: "MemoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Memories_PlatformId",
                table: "Memories",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryTopics_MemoryId",
                table: "MemoryTopics",
                column: "MemoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryTopics_TopicId",
                table: "MemoryTopics",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterPersonas");

            migrationBuilder.DropTable(
                name: "CharacterPlatforms");

            migrationBuilder.DropTable(
                name: "CharacterTopics");

            migrationBuilder.DropTable(
                name: "MemoryTopics");

            migrationBuilder.DropTable(
                name: "Memories");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "CharacterBios");

            migrationBuilder.DropTable(
                name: "MemoryType");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
