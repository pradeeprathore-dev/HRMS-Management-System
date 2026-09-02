using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateITAssetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetCode",
                table: "ITAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ITAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "ITAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "ITAssets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "ITAssets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ITAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "ITAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ITAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetCode",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "ITAssets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ITAssets");
        }
    }
}