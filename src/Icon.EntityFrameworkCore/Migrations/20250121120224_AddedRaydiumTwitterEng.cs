using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Icon.Migrations
{
    /// <inheritdoc />
    public partial class AddedRaydiumTwitterEng : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TwitterCAFirstMention",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterCAFirstMentionText",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterCAFirstMentionTweetId",
                table: "RaydiumPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwitterCAFound",
                table: "RaydiumPairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TwitterCAFoundAtTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TwitterCALookupCount",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TwitterRefreshEnabled",
                table: "RaydiumPairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TwitterRefreshEnabledUntilTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TwitterRefreshIntervalSeconds",
                table: "RaydiumPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TwitterRefreshLastUpdateTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TwitterRefreshNextUpdateTime",
                table: "RaydiumPairs",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwitterCAFirstMention",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCAFirstMentionText",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCAFirstMentionTweetId",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCAFound",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCAFoundAtTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterCALookupCount",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterRefreshEnabled",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterRefreshEnabledUntilTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterRefreshIntervalSeconds",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterRefreshLastUpdateTime",
                table: "RaydiumPairs");

            migrationBuilder.DropColumn(
                name: "TwitterRefreshNextUpdateTime",
                table: "RaydiumPairs");
        }
    }
}
