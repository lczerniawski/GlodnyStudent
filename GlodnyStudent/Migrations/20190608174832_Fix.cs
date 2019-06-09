using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Notifications");

            migrationBuilder.AddColumn<long>(
                name: "RestaurantId",
                table: "Notifications",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Notifications",
                nullable: false,
                defaultValue: "");
        }
    }
}
