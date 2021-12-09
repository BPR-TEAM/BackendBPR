#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrangeBushApi.Migrations
{
    public partial class DashboardUserPLants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DashboardUserPlant",
                columns: table => new
                {
                    DashboardsId = table.Column<int>(type: "integer", nullable: false),
                    UserPlantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardUserPlant", x => new { x.DashboardsId, x.UserPlantsId });
                    table.ForeignKey(
                        name: "FK_DashboardUserPlant_Dashboards_DashboardsId",
                        column: x => x.DashboardsId,
                        principalTable: "Dashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DashboardUserPlant_UserPlants_UserPlantsId",
                        column: x => x.UserPlantsId,
                        principalTable: "UserPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DashboardUserPlant_UserPlantsId",
                table: "DashboardUserPlant",
                column: "UserPlantsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardUserPlant");
        }
    }
}
