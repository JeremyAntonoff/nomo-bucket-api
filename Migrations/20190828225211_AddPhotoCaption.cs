using Microsoft.EntityFrameworkCore.Migrations;

namespace NomoBucket.API.Migrations
{
    public partial class AddPhotoCaption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoCaption",
                table: "BucketListItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoCaption",
                table: "BucketListItems");
        }
    }
}
