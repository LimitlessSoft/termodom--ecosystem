using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class Referent_Connection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Referent",
                table: "Orders",
                newName: "ReferentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReferentId",
                table: "Orders",
                column: "ReferentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_ReferentId",
                table: "Orders",
                column: "ReferentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_ReferentId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReferentId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ReferentId",
                table: "Orders",
                newName: "Referent");
        }
    }
}
