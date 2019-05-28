using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class ReviewsCounterAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "Restaurants",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "Restaurants");
        }
    }
}
