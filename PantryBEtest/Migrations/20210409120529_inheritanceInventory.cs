using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class inheritanceInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Inventory",
                keyColumn: "InventoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PpUser",
                keyColumn: "PpUserId",
                keyValue: 22);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Inventory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "InventoryId", "PpUserId", "Type" },
                values: new object[] { 11, null, null });

            migrationBuilder.InsertData(
                table: "PpUser",
                columns: new[] { "PpUserId", "CreationDate", "Email", "Name", "PasswordHash" },
                values: new object[] { 22, new DateTime(2021, 4, 9, 11, 29, 48, 763, DateTimeKind.Local).AddTicks(4880), "john@john.dk", "John køkkensen", "2233" });
        }
    }
}
