using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class HighestPriceFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "HighestPrice",
                table: "Restaurants",
                type: "decimal(13,2)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "HighestPrice",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13,2)");
        }
    }
}
