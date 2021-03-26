using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class many2many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryCollectionInventoryId",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Item_ItemCollectionItemId",
                table: "InventoryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_ItemCollectionItemId",
                table: "InventoryItem");

            migrationBuilder.RenameColumn(
                name: "ItemCollectionItemId",
                table: "InventoryItem",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "InventoryCollectionInventoryId",
                table: "InventoryItem",
                newName: "InventoryId");

            migrationBuilder.AddColumn<int>(
                name: "amountInFreezer",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "amountInfridge",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Amount",
                table: "InventoryItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryId",
                table: "InventoryItem",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Item_InventoryId",
                table: "InventoryItem",
                column: "InventoryId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Item_InventoryId",
                table: "InventoryItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "amountInFreezer",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "amountInfridge",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "InventoryItem");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "InventoryItem",
                newName: "ItemCollectionItemId");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "InventoryItem",
                newName: "InventoryCollectionInventoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItem",
                table: "InventoryItem",
                columns: new[] { "InventoryCollectionInventoryId", "ItemCollectionItemId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ItemCollectionItemId",
                table: "InventoryItem",
                column: "ItemCollectionItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Inventory_InventoryCollectionInventoryId",
                table: "InventoryItem",
                column: "InventoryCollectionInventoryId",
                principalTable: "Inventory",
                principalColumn: "InventoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Item_ItemCollectionItemId",
                table: "InventoryItem",
                column: "ItemCollectionItemId",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
