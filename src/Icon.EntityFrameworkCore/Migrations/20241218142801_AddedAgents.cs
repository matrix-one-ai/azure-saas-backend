using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedAgents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AgentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AzureAgentTableId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AzureAgentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CharacterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agents");
        }
    }
}
