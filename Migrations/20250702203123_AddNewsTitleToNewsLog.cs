using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newfactjo.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsTitleToNewsLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewsTitle",
                table: "NewsLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsTitle",
                table: "NewsLogs");
        }
    }
}
