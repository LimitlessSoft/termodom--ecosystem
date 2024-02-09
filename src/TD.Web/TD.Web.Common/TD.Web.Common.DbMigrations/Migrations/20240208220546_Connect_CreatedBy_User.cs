using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class Connect_CreatedBy_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedBy",
                table: "Orders",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CreatedBy",
                table: "Orders",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CreatedBy",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CreatedBy",
                table: "Orders");
        }
    }
}
