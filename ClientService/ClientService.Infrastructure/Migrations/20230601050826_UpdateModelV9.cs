using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelV9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Stations_StationId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Stations_StationId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Stations");

            migrationBuilder.CreateTable(
                name: "StationStation",
                columns: table => new
                {
                    NextStationId = table.Column<long>(type: "bigint", nullable: false),
                    PreviousStationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationStation", x => new { x.NextStationId, x.PreviousStationId });
                    table.ForeignKey(
                        name: "FK_StationStation_Stations_NextStationId",
                        column: x => x.NextStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StationStation_Stations_PreviousStationId",
                        column: x => x.PreviousStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StationStation_PreviousStationId",
                table: "StationStation",
                column: "PreviousStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StationStation");

            migrationBuilder.AddColumn<long>(
                name: "StationId",
                table: "Stations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_StationId",
                table: "Stations",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Stations_StationId",
                table: "Stations",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id");
        }
    }
}
