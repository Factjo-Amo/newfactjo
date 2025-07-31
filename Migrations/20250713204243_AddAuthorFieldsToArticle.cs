using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newfactjo.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorFieldsToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorBio",
                table: "Articles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorImageUrl",
                table: "Articles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorBio",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "AuthorImageUrl",
                table: "Articles");
        }
    }
}
