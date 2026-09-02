using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMSAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeOnboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeOnboardings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OfficialEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CredentialsCreated = table.Column<bool>(type: "bit", nullable: false),
                    DocumentsVerified = table.Column<bool>(type: "bit", nullable: false),
                    LaptopAllocated = table.Column<bool>(type: "bit", nullable: false),
                    ManagerAssigned = table.Column<bool>(type: "bit", nullable: false),
                    ProjectAssigned = table.Column<bool>(type: "bit", nullable: false),
                    IdCardGenerated = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOnboardings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeOnboardings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOnboardings_EmployeeId",
                table: "EmployeeOnboardings",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeOnboardings");
        }
    }
}
