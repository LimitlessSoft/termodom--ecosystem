using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class CalculatorItemOrderAndIsPrimaryAddedMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsPrimary",
				table: "CalculatorItems",
				type: "boolean",
				nullable: false,
				defaultValue: false
			);

			migrationBuilder.AddColumn<int>(
				name: "Order",
				table: "CalculatorItems",
				type: "integer",
				nullable: false,
				defaultValue: 0
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "IsPrimary", table: "CalculatorItems");

			migrationBuilder.DropColumn(name: "Order", table: "CalculatorItems");
		}
	}
}
