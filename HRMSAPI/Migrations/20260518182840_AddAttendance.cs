using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckIn",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckOut",
                table: "Attendances");

            migrationBuilder.AddColumn<DateTime>(
                name: "PunchIn",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PunchOut",
                table: "Attendances",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PunchIn",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "PunchOut",
                table: "Attendances");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckIn",
                table: "Attendances",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CheckOut",
                table: "Attendances",
                type: "time",
                nullable: true);
        }
    }
}
