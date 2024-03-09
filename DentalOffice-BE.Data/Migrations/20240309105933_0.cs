using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DentalOffice_BE.Data.Migrations
{
    /// <inheritdoc />
    public partial class _0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "main");

            migrationBuilder.CreateTable(
                name: "colors",
                schema: "main",
                columns: table => new
                {
                    code = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()"),
                    id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.code);
                    table.UniqueConstraint("AK_colors_id", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "document_configurations",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_configurations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "material_types",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "risks",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_risks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    section_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    route = table.Column<string>(type: "text", nullable: false),
                    api_string = table.Column<string>(type: "text", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()"),
                    Configuration = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sections", x => x.id);
                    table.ForeignKey(
                        name: "FK_sections_sections_section_id",
                        column: x => x.section_id,
                        principalSchema: "main",
                        principalTable: "sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "semiproducts",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semiproducts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stages",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "studios",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    material_type_id = table.Column<long>(type: "bigint", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()"),
                    MaterialProperties = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.id);
                    table.ForeignKey(
                        name: "FK_materials_material_types_material_type_id",
                        column: x => x.material_type_id,
                        principalSchema: "main",
                        principalTable: "material_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_name = table.Column<string>(type: "text", nullable: false),
                    prescription_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    delivery_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    studio_id = table.Column<long>(type: "bigint", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.id);
                    table.ForeignKey(
                        name: "FK_modules_studios_studio_id",
                        column: x => x.studio_id,
                        principalSchema: "main",
                        principalTable: "studios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lots",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    material_id = table.Column<long>(type: "bigint", nullable: false),
                    ColorDtoCode = table.Column<string>(type: "text", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lots", x => x.id);
                    table.ForeignKey(
                        name: "FK_lots_colors_ColorDtoCode",
                        column: x => x.ColorDtoCode,
                        principalSchema: "main",
                        principalTable: "colors",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_lots_materials_material_id",
                        column: x => x.material_id,
                        principalSchema: "main",
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "document_instances",
                schema: "main",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    configuration_id = table.Column<long>(type: "bigint", nullable: false),
                    module_id = table.Column<long>(type: "bigint", nullable: false),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_instances", x => x.id);
                    table.ForeignKey(
                        name: "FK_document_instances_document_configurations_configuration_id",
                        column: x => x.configuration_id,
                        principalSchema: "main",
                        principalTable: "document_configurations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_document_instances_modules_module_id",
                        column: x => x.module_id,
                        principalSchema: "main",
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    semiproduct_id = table.Column<long>(type: "bigint", nullable: true),
                    metal_material_id = table.Column<long>(type: "bigint", nullable: false),
                    metal_lot_id = table.Column<long>(type: "bigint", nullable: false),
                    dentin_material_id = table.Column<long>(type: "bigint", nullable: false),
                    dentin_lot_id = table.Column<long>(type: "bigint", nullable: false),
                    enamel_material_id = table.Column<long>(type: "bigint", nullable: false),
                    enamel_lot_id = table.Column<long>(type: "bigint", nullable: false),
                    risk_id = table.Column<long>(type: "bigint", nullable: false),
                    color_id = table.Column<string>(type: "text", nullable: false),
                    ColorDtoCode = table.Column<string>(type: "text", nullable: true),
                    ModuleDtoId = table.Column<long>(type: "bigint", nullable: true),
                    RiskDtoId = table.Column<long>(type: "bigint", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    update_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Processes_colors_ColorDtoCode",
                        column: x => x.ColorDtoCode,
                        principalSchema: "main",
                        principalTable: "colors",
                        principalColumn: "code");
                    table.ForeignKey(
                        name: "FK_Processes_colors_color_id",
                        column: x => x.color_id,
                        principalSchema: "main",
                        principalTable: "colors",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_lots_dentin_lot_id",
                        column: x => x.dentin_lot_id,
                        principalSchema: "main",
                        principalTable: "lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_lots_enamel_lot_id",
                        column: x => x.enamel_lot_id,
                        principalSchema: "main",
                        principalTable: "lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_lots_metal_lot_id",
                        column: x => x.metal_lot_id,
                        principalSchema: "main",
                        principalTable: "lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_materials_dentin_material_id",
                        column: x => x.dentin_material_id,
                        principalSchema: "main",
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_materials_enamel_material_id",
                        column: x => x.enamel_material_id,
                        principalSchema: "main",
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_materials_metal_material_id",
                        column: x => x.metal_material_id,
                        principalSchema: "main",
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_modules_ModuleDtoId",
                        column: x => x.ModuleDtoId,
                        principalSchema: "main",
                        principalTable: "modules",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Processes_modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "main",
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_risks_RiskDtoId",
                        column: x => x.RiskDtoId,
                        principalSchema: "main",
                        principalTable: "risks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Processes_risks_risk_id",
                        column: x => x.risk_id,
                        principalSchema: "main",
                        principalTable: "risks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processes_semiproducts_semiproduct_id",
                        column: x => x.semiproduct_id,
                        principalSchema: "main",
                        principalTable: "semiproducts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "processes_stages",
                columns: table => new
                {
                    ProcessesId = table.Column<long>(type: "bigint", nullable: false),
                    StagesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processes_stages", x => new { x.ProcessesId, x.StagesId });
                    table.ForeignKey(
                        name: "FK_processes_stages_Processes_ProcessesId",
                        column: x => x.ProcessesId,
                        principalTable: "Processes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_processes_stages_stages_StagesId",
                        column: x => x.StagesId,
                        principalSchema: "main",
                        principalTable: "stages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customers_name",
                schema: "main",
                table: "customers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_configurations_name",
                schema: "main",
                table: "document_configurations",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_document_instances_configuration_id",
                schema: "main",
                table: "document_instances",
                column: "configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_document_instances_module_id",
                schema: "main",
                table: "document_instances",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_lots_ColorDtoCode",
                schema: "main",
                table: "lots",
                column: "ColorDtoCode");

            migrationBuilder.CreateIndex(
                name: "IX_lots_material_id",
                schema: "main",
                table: "lots",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_types_name",
                schema: "main",
                table: "material_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_materials_material_type_id",
                schema: "main",
                table: "materials",
                column: "material_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_materials_name",
                schema: "main",
                table: "materials",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_modules_studio_id",
                schema: "main",
                table: "modules",
                column: "studio_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_color_id",
                table: "Processes",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ColorDtoCode",
                table: "Processes",
                column: "ColorDtoCode");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_dentin_lot_id",
                table: "Processes",
                column: "dentin_lot_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_dentin_material_id",
                table: "Processes",
                column: "dentin_material_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_enamel_lot_id",
                table: "Processes",
                column: "enamel_lot_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_enamel_material_id",
                table: "Processes",
                column: "enamel_material_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_metal_lot_id",
                table: "Processes",
                column: "metal_lot_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_metal_material_id",
                table: "Processes",
                column: "metal_material_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ModuleDtoId",
                table: "Processes",
                column: "ModuleDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_ModuleId",
                table: "Processes",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_risk_id",
                table: "Processes",
                column: "risk_id");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_RiskDtoId",
                table: "Processes",
                column: "RiskDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_semiproduct_id",
                table: "Processes",
                column: "semiproduct_id");

            migrationBuilder.CreateIndex(
                name: "IX_processes_stages_StagesId",
                table: "processes_stages",
                column: "StagesId");

            migrationBuilder.CreateIndex(
                name: "IX_risks_description",
                schema: "main",
                table: "risks",
                column: "description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sections_route",
                schema: "main",
                table: "sections",
                column: "route",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sections_section_id",
                schema: "main",
                table: "sections",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_semiproducts_name",
                schema: "main",
                table: "semiproducts",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_stages_name",
                schema: "main",
                table: "stages",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_studios_name",
                schema: "main",
                table: "studios",
                column: "name",
                unique: true);

            migrationBuilder.Sql(Properties.Resources.init_data);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers",
                schema: "main");

            migrationBuilder.DropTable(
                name: "document_instances",
                schema: "main");

            migrationBuilder.DropTable(
                name: "processes_stages");

            migrationBuilder.DropTable(
                name: "sections",
                schema: "main");

            migrationBuilder.DropTable(
                name: "document_configurations",
                schema: "main");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "stages",
                schema: "main");

            migrationBuilder.DropTable(
                name: "lots",
                schema: "main");

            migrationBuilder.DropTable(
                name: "modules",
                schema: "main");

            migrationBuilder.DropTable(
                name: "risks",
                schema: "main");

            migrationBuilder.DropTable(
                name: "semiproducts",
                schema: "main");

            migrationBuilder.DropTable(
                name: "colors",
                schema: "main");

            migrationBuilder.DropTable(
                name: "materials",
                schema: "main");

            migrationBuilder.DropTable(
                name: "studios",
                schema: "main");

            migrationBuilder.DropTable(
                name: "material_types",
                schema: "main");
        }
    }
}
