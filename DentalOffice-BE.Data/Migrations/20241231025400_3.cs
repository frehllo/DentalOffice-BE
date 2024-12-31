using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalOffice_BE.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processes_colors_color_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_dentin_lot_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_enamel_lot_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_metal_lot_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_materials_dentin_material_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_materials_metal_material_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_risks_risk_id",
                schema: "main",
                table: "processes");

            migrationBuilder.AlterColumn<long>(
                name: "risk_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "metal_material_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "metal_lot_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "enamel_lot_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "dentin_material_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "dentin_lot_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "color_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_colors_color_id",
                schema: "main",
                table: "processes",
                column: "color_id",
                principalSchema: "main",
                principalTable: "colors",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_dentin_lot_id",
                schema: "main",
                table: "processes",
                column: "dentin_lot_id",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_enamel_lot_id",
                schema: "main",
                table: "processes",
                column: "enamel_lot_id",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_metal_lot_id",
                schema: "main",
                table: "processes",
                column: "metal_lot_id",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_materials_dentin_material_id",
                schema: "main",
                table: "processes",
                column: "dentin_material_id",
                principalSchema: "main",
                principalTable: "materials",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_materials_metal_material_id",
                schema: "main",
                table: "processes",
                column: "metal_material_id",
                principalSchema: "main",
                principalTable: "materials",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_processes_risks_risk_id",
                schema: "main",
                table: "processes",
                column: "risk_id",
                principalSchema: "main",
                principalTable: "risks",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processes_colors_color_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_dentin_lot_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_enamel_lot_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_lots_metal_lot_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_materials_dentin_material_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_materials_metal_material_id",
                schema: "main",
                table: "processes");

            migrationBuilder.DropForeignKey(
                name: "FK_processes_risks_risk_id",
                schema: "main",
                table: "processes");

            migrationBuilder.AlterColumn<long>(
                name: "risk_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "metal_material_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "metal_lot_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "enamel_lot_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "dentin_material_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "dentin_lot_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "color_id",
                schema: "main",
                table: "processes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_colors_color_id",
                schema: "main",
                table: "processes",
                column: "color_id",
                principalSchema: "main",
                principalTable: "colors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_dentin_lot_id",
                schema: "main",
                table: "processes",
                column: "dentin_lot_id",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_enamel_lot_id",
                schema: "main",
                table: "processes",
                column: "enamel_lot_id",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_lots_metal_lot_id",
                schema: "main",
                table: "processes",
                column: "metal_lot_id",
                principalSchema: "main",
                principalTable: "lots",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_materials_dentin_material_id",
                schema: "main",
                table: "processes",
                column: "dentin_material_id",
                principalSchema: "main",
                principalTable: "materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_materials_metal_material_id",
                schema: "main",
                table: "processes",
                column: "metal_material_id",
                principalSchema: "main",
                principalTable: "materials",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processes_risks_risk_id",
                schema: "main",
                table: "processes",
                column: "risk_id",
                principalSchema: "main",
                principalTable: "risks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
