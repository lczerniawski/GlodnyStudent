using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressCoordinates");

            migrationBuilder.DropColumn(
                name: "City",
                table: "RestaurantAddresses");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "RestaurantAddresses");

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

            migrationBuilder.AddColumn<IPoint>(
                name: "Location",
                table: "RestaurantAddresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "RestaurantAddresses");

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

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "RestaurantAddresses",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "RestaurantAddresses",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AddressCoordinates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    RestaurantAddressId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressCoordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressCoordinates_RestaurantAddresses_RestaurantAddressId",
                        column: x => x.RestaurantAddressId,
                        principalTable: "RestaurantAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressCoordinates_RestaurantAddressId",
                table: "AddressCoordinates",
                column: "RestaurantAddressId",
                unique: true);
        }
    }
}
