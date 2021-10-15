#pragma warning disable 1591
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrangeBushApi.Migrations
{
    public partial class Inheritanceformeasurementdefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserPlantId",
                table: "Measurements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "MeasurementDefinitions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserPlantId",
                table: "MeasurementDefinitions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_UserPlantId",
                table: "Measurements",
                column: "UserPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementDefinitions_UserPlantId",
                table: "MeasurementDefinitions",
                column: "UserPlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementDefinitions_UserPlants_UserPlantId",
                table: "MeasurementDefinitions",
                column: "UserPlantId",
                principalTable: "UserPlants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_UserPlants_UserPlantId",
                table: "Measurements",
                column: "UserPlantId",
                principalTable: "UserPlants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementDefinitions_UserPlants_UserPlantId",
                table: "MeasurementDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_UserPlants_UserPlantId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_UserPlantId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_MeasurementDefinitions_UserPlantId",
                table: "MeasurementDefinitions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserPlantId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "MeasurementDefinitions");

            migrationBuilder.DropColumn(
                name: "UserPlantId",
                table: "MeasurementDefinitions");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
