using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Admin.Api.Migrations
{
    public partial class ProductEntityEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitEntityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitEntityId",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitEntityId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitEntityId",
                table: "Products",
                column: "UnitEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitEntityId",
                table: "Products",
                column: "UnitEntityId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}
