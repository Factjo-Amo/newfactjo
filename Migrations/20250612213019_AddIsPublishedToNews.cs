using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Newfactjo.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPublishedToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "NewsItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // ✅ تفعيل جميع الأخبار السابقة تلقائيًا
            migrationBuilder.Sql("UPDATE NewsItems SET IsPublished = 1");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "NewsItems");
        }
    }
}
