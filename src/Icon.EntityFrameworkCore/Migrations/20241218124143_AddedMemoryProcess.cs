using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedMemoryProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoryActions");

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryProcessId",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MemoryProcessId1",
                table: "Memories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MemoryProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MemoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryProcesses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MemoryProcessSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MemoryProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParametersJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    DependenciesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    MaxRetries = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryProcessSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryProcessSteps_MemoryProcesses_MemoryProcessId",
                        column: x => x.MemoryProcessId,
                        principalTable: "MemoryProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemoryProcessLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    MemoryProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemoryProcessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoggedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LogLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryProcessLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemoryProcessLogs_MemoryProcessSteps_MemoryProcessStepId",
                        column: x => x.MemoryProcessStepId,
                        principalTable: "MemoryProcessSteps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MemoryProcessLogs_MemoryProcesses_MemoryProcessId",
                        column: x => x.MemoryProcessId,
                        principalTable: "MemoryProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memories_MemoryProcessId1",
                table: "Memories",
                column: "MemoryProcessId1");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryProcessLogs_MemoryProcessId",
                table: "MemoryProcessLogs",
                column: "MemoryProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryProcessLogs_MemoryProcessStepId",
                table: "MemoryProcessLogs",
                column: "MemoryProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_MemoryProcessSteps_MemoryProcessId",
                table: "MemoryProcessSteps",
                column: "MemoryProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memories_MemoryProcesses_MemoryProcessId1",
                table: "Memories",
                column: "MemoryProcessId1",
                principalTable: "MemoryProcesses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memories_MemoryProcesses_MemoryProcessId1",
                table: "Memories");

            migrationBuilder.DropTable(
                name: "MemoryProcessLogs");

            migrationBuilder.DropTable(
                name: "MemoryProcessSteps");

            migrationBuilder.DropTable(
                name: "MemoryProcesses");

            migrationBuilder.DropIndex(
                name: "IX_Memories_MemoryProcessId1",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryProcessId",
                table: "Memories");

            migrationBuilder.DropColumn(
                name: "MemoryProcessId1",
                table: "Memories");

            migrationBuilder.CreateTable(
                name: "MemoryActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    MemoryPromptId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MemoryActions_MemoryId",
                table: "MemoryActions",
                column: "MemoryId");
        }
    }
}
