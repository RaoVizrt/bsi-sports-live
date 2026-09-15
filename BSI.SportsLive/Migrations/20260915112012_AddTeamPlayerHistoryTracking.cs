using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSI.SportsLive.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamPlayerHistoryTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TeamPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedDate",
                table: "TeamPlayers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LeftDate",
                table: "TeamPlayers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPlayers_TeamId",
                table: "TeamPlayers",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers");

            migrationBuilder.DropIndex(
                name: "IX_TeamPlayers_TeamId",
                table: "TeamPlayers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TeamPlayers");

            migrationBuilder.DropColumn(
                name: "JoinedDate",
                table: "TeamPlayers");

            migrationBuilder.DropColumn(
                name: "LeftDate",
                table: "TeamPlayers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamPlayers",
                table: "TeamPlayers",
                columns: new[] { "TeamId", "PlayerId" });
        }
    }
}
