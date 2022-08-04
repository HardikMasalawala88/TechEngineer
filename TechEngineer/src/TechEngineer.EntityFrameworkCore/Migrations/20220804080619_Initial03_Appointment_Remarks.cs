using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechEngineer.Migrations
{
    public partial class Initial03_Appointment_Remarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Appointments");
        }
    }
}
