using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class CalculatorItemIsHobyStandardProfiMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "IsHobi",
				table: "CalculatorItems",
				type: "boolean",
				nullable: false,
				defaultValue: false
			);

			migrationBuilder.AddColumn<bool>(
				name: "IsProfi",
				table: "CalculatorItems",
				type: "boolean",
				nullable: false,
				defaultValue: false
			);

			migrationBuilder.AddColumn<bool>(
				name: "IsStandard",
				table: "CalculatorItems",
				type: "boolean",
				nullable: false,
				defaultValue: false
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "IsHobi", table: "CalculatorItems");

			migrationBuilder.DropColumn(name: "IsProfi", table: "CalculatorItems");

			migrationBuilder.DropColumn(name: "IsStandard", table: "CalculatorItems");
		}
	}
}
