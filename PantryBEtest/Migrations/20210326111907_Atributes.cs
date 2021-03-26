using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class Atributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amountInfridge",
                table: "Item",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "amountInFreezer",
                table: "Item",
                newName: "AverageLifespanDays");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "PpUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "InventoryItem",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "PpUser");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "InventoryItem");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Item",
                newName: "amountInfridge");

            migrationBuilder.RenameColumn(
                name: "AverageLifespanDays",
                table: "Item",
                newName: "amountInFreezer");
        }
    }
}
