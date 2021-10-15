#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrangeBushApi.Migrations
{
    public partial class imageattributeonuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Users",
                type: "bytea",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Users");
        }
    }
}
