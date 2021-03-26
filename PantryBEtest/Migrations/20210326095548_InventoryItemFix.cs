using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class InventoryItemFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Item_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem",
                columns: new[] { "InventoryId", "ItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ItemId",
                table: "InventoryItem",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Item_ItemId",
                table: "InventoryItem",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Item_ItemId",
                table: "InventoryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_ItemId",
                table: "InventoryItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Item_InventoryId",
                table: "InventoryItem",
                column: "InventoryId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
