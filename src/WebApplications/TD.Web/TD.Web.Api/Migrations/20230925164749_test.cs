using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Api.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPriceEntity_PriceId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPriceEntity",
                table: "ProductPriceEntity");

            migrationBuilder.RenameTable(
                name: "ProductPriceEntity",
                newName: "ProductPrices");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductPrices",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPrices_PriceId",
                table: "Products",
                column: "PriceId",
                principalTable: "ProductPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductPrices_PriceId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices");

            migrationBuilder.RenameTable(
                name: "ProductPrices",
                newName: "ProductPriceEntity");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "ProductPriceEntity",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPriceEntity",
                table: "ProductPriceEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductPriceEntity_PriceId",
                table: "Products",
                column: "PriceId",
                principalTable: "ProductPriceEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
