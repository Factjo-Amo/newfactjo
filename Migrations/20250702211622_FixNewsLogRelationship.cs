using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newfactjo.Migrations
{
    /// <inheritdoc />
    public partial class FixNewsLogRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs",
                column: "NewsId",
                principalTable: "NewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs",
                column: "NewsId",
                principalTable: "NewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
