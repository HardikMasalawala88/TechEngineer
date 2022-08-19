using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechEngineer.Migrations
{
    public partial class Initial03_BranchITHead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchITHeadEmail",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchITHeadEmail",
                table: "Locations");
        }
    }
}
