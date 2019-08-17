using Microsoft.EntityFrameworkCore.Migrations;

namespace NomoBucket.API.Migrations
{
    public partial class AddPublicPhotoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicPhotoId",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicPhotoId",
                table: "Users");
        }
    }
}
