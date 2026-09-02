using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class HolidayEnhancement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Holidays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOptional",
                table: "Holidays",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "IsOptional",
                table: "Holidays");
        }
    }
}
