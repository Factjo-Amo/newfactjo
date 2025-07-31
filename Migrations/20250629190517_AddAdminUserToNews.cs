using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newfactjo.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUserToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "NewsItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_AdminUserId",
                table: "NewsItems",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsItems_AdminUsers_AdminUserId",
                table: "NewsItems",
                column: "AdminUserId",
                principalTable: "AdminUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsItems_AdminUsers_AdminUserId",
                table: "NewsItems");

            migrationBuilder.DropIndex(
                name: "IX_NewsItems_AdminUserId",
                table: "NewsItems");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "NewsItems");
        }
    }
}
