using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClientService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Trips_TripId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Posts_PostId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_PostId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TripId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Posts");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Trips",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Posts_Id",
                table: "Trips",
                column: "Id",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Posts_Id",
                table: "Trips");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Trips",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "PostId",
                table: "Trips",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TripId",
                table: "Posts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_PostId",
                table: "Trips",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TripId",
                table: "Posts",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Trips_TripId",
                table: "Posts",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Posts_PostId",
                table: "Trips",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
