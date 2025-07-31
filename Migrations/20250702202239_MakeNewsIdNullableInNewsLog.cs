using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newfactjo.Migrations
{
    /// <inheritdoc />
    public partial class MakeNewsIdNullableInNewsLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs");

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "NewsLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs",
                column: "NewsId",
                principalTable: "NewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs");

            migrationBuilder.AlterColumn<int>(
                name: "NewsId",
                table: "NewsLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NewsLogs_NewsItems_NewsId",
                table: "NewsLogs",
                column: "NewsId",
                principalTable: "NewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
