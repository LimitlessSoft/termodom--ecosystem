using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Api.Migrations
{
    public partial class addProductPriceGrouptoProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductPriceGroupId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductPriceGroupId",
                table: "Products",
                column: "ProductPriceGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPriceGroupEntities_ProductPriceGroupId",
                table: "Products",
                column: "ProductPriceGroupId",
                principalTable: "ProductPriceGroupEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPriceGroupEntities_ProductPriceGroupId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductPriceGroupId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductPriceGroupId",
                table: "Products");
        }
    }
}
