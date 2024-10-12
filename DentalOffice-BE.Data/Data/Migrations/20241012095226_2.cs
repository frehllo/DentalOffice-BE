using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalOffice_BE.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CopyCount",
                schema: "main",
                table: "document_configurations",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CopyCount",
                schema: "main",
                table: "document_configurations");
        }
    }
}
