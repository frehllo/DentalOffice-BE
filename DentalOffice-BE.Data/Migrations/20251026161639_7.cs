using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalOffice_BE.Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Others",
                schema: "main",
                table: "processes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Others",
                schema: "main",
                table: "processes");
        }
    }
}
