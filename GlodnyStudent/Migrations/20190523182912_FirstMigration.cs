using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(16)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(16)",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)");
        }
    }
}
