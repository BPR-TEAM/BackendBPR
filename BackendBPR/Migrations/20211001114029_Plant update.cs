using Microsoft.EntityFrameworkCore.Migrations;

namespace OrangeBushApi.Migrations
{
    public partial class Plantupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Plants",
                newName: "ScientificName");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommonName",
                table: "Plants",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CommonName",
                table: "Plants");

            migrationBuilder.RenameColumn(
                name: "ScientificName",
                table: "Plants",
                newName: "Name");
        }
    }
}
