using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClientService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Stations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Stations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Stations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ObjectStatus",
                table: "Stations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "StationId",
                table: "Stations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PostId",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripRole = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TripId = table.Column<long>(type: "bigint", nullable: true),
                    FeedbackPoint = table.Column<float>(type: "real", nullable: false),
                    FeedbackContent = table.Column<string>(type: "text", nullable: false),
                    StartStationId = table.Column<long>(type: "bigint", nullable: false),
                    EndStationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Account_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Stations_EndStationId",
                        column: x => x.EndStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Stations_StartStationId",
                        column: x => x.StartStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PassengerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GrabberId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripStatus = table.Column<int>(type: "integer", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FinishAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CancelAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PostedStartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FeedbackPoint = table.Column<float>(type: "real", nullable: false),
                    FeedbackContent = table.Column<string>(type: "text", nullable: false),
                    StartStationId = table.Column<long>(type: "bigint", nullable: false),
                    EndStationId = table.Column<long>(type: "bigint", nullable: false),
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Account_GrabberId",
                        column: x => x.GrabberId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Account_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Stations_EndStationId",
                        column: x => x.EndStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Stations_StartStationId",
                        column: x => x.StartStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_StationId",
                table: "Stations",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_PostId",
                table: "Account",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_EndStationId",
                table: "Posts",
                column: "EndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_StartStationId",
                table: "Posts",
                column: "StartStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TripId",
                table: "Posts",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_EndStationId",
                table: "Trips",
                column: "EndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_GrabberId",
                table: "Trips",
                column: "GrabberId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PassengerId",
                table: "Trips",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PostId",
                table: "Trips",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_StartStationId",
                table: "Trips",
                column: "StartStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Posts_PostId",
                table: "Account",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Stations_StationId",
                table: "Stations",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Trips_TripId",
                table: "Posts",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Posts_PostId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Stations_StationId",
                table: "Stations");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Trips_TripId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Stations_StationId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_Account_PostId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "ObjectStatus",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Account");
        }
    }
}
