using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GlodnyStudent.Migrations
{
    public partial class ImageRepoUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Images");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageSource",
                table: "Images",
                nullable: true);
        }
    }
}
