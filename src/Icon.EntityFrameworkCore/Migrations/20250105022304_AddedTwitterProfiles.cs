using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwitterProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterPersonaTwitterProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterPersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowersCount = table.Column<int>(type: "int", nullable: true),
                    FollowingCount = table.Column<int>(type: "int", nullable: true),
                    FriendsCount = table.Column<int>(type: "int", nullable: true),
                    MediaCount = table.Column<int>(type: "int", nullable: true),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: true),
                    ListedCount = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PinnedTweetIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TweetsCount = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBlueVerified = table.Column<bool>(type: "bit", nullable: true),
                    CanDm = table.Column<bool>(type: "bit", nullable: true),
                    Joined = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastImportDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPersonaTwitterProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterPersonaTwitterProfile_CharacterPersonas_CharacterPersonaId",
                        column: x => x.CharacterPersonaId,
                        principalTable: "CharacterPersonas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterPersonaTwitterProfile_CharacterPersonaId",
                table: "CharacterPersonaTwitterProfile",
                column: "CharacterPersonaId",
                unique: true,
                filter: "[CharacterPersonaId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterPersonaTwitterProfile");
        }
    }
}
