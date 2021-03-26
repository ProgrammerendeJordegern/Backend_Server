using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class userID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_PpUser_PpUserEmail",
                table: "Inventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PpUser",
                table: "PpUser");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_PpUserEmail",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PpUserEmail",
                table: "Inventory");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PpUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PpUserId",
                table: "PpUser",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PpUserId",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PpUser",
                table: "PpUser",
                column: "PpUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_PpUserId",
                table: "Inventory",
                column: "PpUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_PpUser_PpUserId",
                table: "Inventory",
                column: "PpUserId",
                principalTable: "PpUser",
                principalColumn: "PpUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_PpUser_PpUserId",
                table: "Inventory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PpUser",
                table: "PpUser");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_PpUserId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PpUserId",
                table: "PpUser");

            migrationBuilder.DropColumn(
                name: "PpUserId",
                table: "Inventory");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "PpUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PpUserEmail",
                table: "Inventory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PpUser",
                table: "PpUser",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_PpUserEmail",
                table: "Inventory",
                column: "PpUserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_PpUser_PpUserEmail",
                table: "Inventory",
                column: "PpUserEmail",
                principalTable: "PpUser",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
