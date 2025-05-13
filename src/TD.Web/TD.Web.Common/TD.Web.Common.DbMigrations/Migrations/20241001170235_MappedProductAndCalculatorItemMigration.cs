using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class MappedProductAndCalculatorItemMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_CalculatorItems_ProductId",
				table: "CalculatorItems",
				column: "ProductId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_CalculatorItems_Products_ProductId",
				table: "CalculatorItems",
				column: "ProductId",
				principalTable: "Products",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_CalculatorItems_Products_ProductId",
				table: "CalculatorItems"
			);

			migrationBuilder.DropIndex(
				name: "IX_CalculatorItems_ProductId",
				table: "CalculatorItems"
			);
		}
	}
}
