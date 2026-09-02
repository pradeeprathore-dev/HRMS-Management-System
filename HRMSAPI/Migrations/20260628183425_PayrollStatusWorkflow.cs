using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class PayrollStatusWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Payrolls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payrolls",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payrolls",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payrolls");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payrolls");
        }
    }
}
