using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class ProductAlternateUnit : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "AlternateUnitId",
				table: "Products",
				type: "integer",
				nullable: true
			);

			migrationBuilder.AddColumn<decimal>(
				name: "OneAlternatePackageEquals",
				table: "Products",
				type: "numeric",
				nullable: true
			);

			migrationBuilder.CreateIndex(
				name: "IX_Products_AlternateUnitId",
				table: "Products",
				column: "AlternateUnitId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_Products_Units_AlternateUnitId",
				table: "Products",
				column: "AlternateUnitId",
				principalTable: "Units",
				principalColumn: "Id"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Products_Units_AlternateUnitId",
				table: "Products"
			);

			migrationBuilder.DropIndex(name: "IX_Products_AlternateUnitId", table: "Products");

			migrationBuilder.DropColumn(name: "AlternateUnitId", table: "Products");

			migrationBuilder.DropColumn(name: "OneAlternatePackageEquals", table: "Products");
		}
	}
}
