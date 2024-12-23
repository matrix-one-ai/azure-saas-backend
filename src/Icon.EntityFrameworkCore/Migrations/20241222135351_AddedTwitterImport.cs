using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwitterImport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TwitterUserName",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TwitterImportLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TwitterAgentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoggedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterImportLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterImportTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TwitterAgentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportLimitTotal = table.Column<int>(type: "int", nullable: false),
                    LastRunCompletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastRunDurationSeconds = table.Column<int>(type: "int", nullable: false),
                    LastRunStartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextRunTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RunEveryXMinutes = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterImportTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterImportTweets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookmarkCount = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hashtags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Html = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InReplyToStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsQuoted = table.Column<bool>(type: "bit", nullable: false),
                    IsPin = table.Column<bool>(type: "bit", nullable: false),
                    IsReply = table.Column<bool>(type: "bit", nullable: false),
                    IsRetweet = table.Column<bool>(type: "bit", nullable: false),
                    IsSelfThread = table.Column<bool>(type: "bit", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotedStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Replies = table.Column<int>(type: "int", nullable: false),
                    Retweets = table.Column<int>(type: "int", nullable: false),
                    RetweetedStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeParsed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Urls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Views = table.Column<int>(type: "int", nullable: false),
                    SensitiveContent = table.Column<bool>(type: "bit", nullable: false),
                    MentionsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotosJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideosJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PollJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InReplyToStatusJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotedStatusJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetweetedStatusJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThreadJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exported = table.Column<bool>(type: "bit", nullable: false),
                    ExportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastTwitterImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastTwitterImportExported = table.Column<bool>(type: "bit", nullable: false),
                    TweetId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterImportTweets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitterImportLogs");

            migrationBuilder.DropTable(
                name: "TwitterImportTasks");

            migrationBuilder.DropTable(
                name: "TwitterImportTweets");

            migrationBuilder.DropColumn(
                name: "TwitterUserName",
                table: "Characters");
        }
    }
}
