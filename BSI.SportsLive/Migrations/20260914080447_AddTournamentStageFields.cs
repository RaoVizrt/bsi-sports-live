using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSI.SportsLive.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentStageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParticipantsCount",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sport",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StageFormat",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantsCount",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Sport",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "StageFormat",
                table: "Tournaments");
        }
    }
}
