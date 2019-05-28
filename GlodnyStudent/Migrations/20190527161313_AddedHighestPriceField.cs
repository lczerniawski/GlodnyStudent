using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class AddedHighestPriceField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HighestPrice",
                table: "Restaurants",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestPrice",
                table: "Restaurants");
        }
    }
}
