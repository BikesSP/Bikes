using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelV8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Posts_PostId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_PostId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Account");

            migrationBuilder.CreateTable(
                name: "AccountPost",
                columns: table => new
                {
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ApplierId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPost", x => new { x.ApplicationId, x.ApplierId });
                    table.ForeignKey(
                        name: "FK_AccountPost_Account_ApplierId",
                        column: x => x.ApplierId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountPost_Posts_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPost_ApplierId",
                table: "AccountPost",
                column: "ApplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPost");

            migrationBuilder.AddColumn<long>(
                name: "PostId",
                table: "Account",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_PostId",
                table: "Account",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Posts_PostId",
                table: "Account",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
