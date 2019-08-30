using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NomoBucket.API.Migrations
{
    public partial class AddFeedItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    ItemCreatedAt = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CompletedPhotoUrl = table.Column<string>(nullable: true),
                    PhotoCaption = table.Column<string>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedItems");
        }
    }
}
