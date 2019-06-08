using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class CuisineEnityFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cuisines",
                table: "Cuisines");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cuisines",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Cuisines",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cuisines",
                table: "Cuisines",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cuisines",
                table: "Cuisines");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Cuisines");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cuisines",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cuisines",
                table: "Cuisines",
                column: "Name");
        }
    }
}
