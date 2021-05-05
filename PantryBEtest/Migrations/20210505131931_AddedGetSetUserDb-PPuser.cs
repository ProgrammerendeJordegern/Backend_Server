using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class AddedGetSetUserDbPPuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PpUserId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PpUserId",
                table: "User",
                column: "PpUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_PpUser_PpUserId",
                table: "User",
                column: "PpUserId",
                principalTable: "PpUser",
                principalColumn: "PpUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_PpUser_PpUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PpUserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PpUserId",
                table: "User");
        }
    }
}
