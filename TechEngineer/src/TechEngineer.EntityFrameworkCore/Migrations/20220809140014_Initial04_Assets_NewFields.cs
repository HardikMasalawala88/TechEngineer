using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechEngineer.Migrations
{
    public partial class Initial04_Assets_NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPU",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HDD",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KeyBoard",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MSOffice",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Monitor",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Monitor_SerialNo",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MotherBoard",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mouse",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RAM",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "System_Username",
                table: "Assets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CPU",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "HDD",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "KeyBoard",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "MSOffice",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Monitor",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Monitor_SerialNo",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "MotherBoard",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Mouse",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "RAM",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "System_Username",
                table: "Assets");
        }
    }
}
