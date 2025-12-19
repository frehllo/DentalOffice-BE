using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalOffice_BE.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DentinCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DentinLotCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiskCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiskLotCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DiskLotId",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DiskMaterialId",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnamelCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnamelLotCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetalCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetalLotCustom",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_processes_DiskLotId",
                schema: "main",
                table: "processes",
                column: "DiskLotId");

            migrationBuilder.CreateIndex(
                name: "IX_processes_DiskMaterialId",
                schema: "main",
                table: "processes",
                column: "DiskMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_DiskLotId",
                schema: "main",
                table: "processes",
                column: "DiskLotId",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_materials_DiskMaterialId",
                schema: "main",
                table: "processes",
                column: "DiskMaterialId",
                principalSchema: "main",
                principalTable: "materials",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_DiskLotId",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_materials_DiskMaterialId",
                schema: "main",
                table: "processes");

            migrationBuilder.DropIndex(
                name: "IX_processes_DiskLotId",
                schema: "main",
                table: "processes");

            migrationBuilder.DropIndex(
                name: "IX_processes_DiskMaterialId",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "DentinCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "DentinLotCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "DiskCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "DiskLotCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "DiskLotId",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "DiskMaterialId",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "EnamelCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "EnamelLotCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "MetalCustom",
                schema: "main",
                table: "processes");

            migrationBuilder.DropColumn(
                name: "MetalLotCustom",
                schema: "main",
                table: "processes");
        }
    }
}
