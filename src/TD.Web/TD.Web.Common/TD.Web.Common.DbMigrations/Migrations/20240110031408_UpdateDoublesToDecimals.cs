using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class UpdateDoublesToDecimals : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<decimal>(
				name: "ProdajnaCenaBezPDV",
				table: "KomercijalnoPrices",
				type: "numeric",
				nullable: false,
				oldClrType: typeof(double),
				oldType: "double precision"
			);

			migrationBuilder.AlterColumn<decimal>(
				name: "NabavnaCenaBezPDV",
				table: "KomercijalnoPrices",
				type: "numeric",
				nullable: false,
				oldClrType: typeof(double),
				oldType: "double precision"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<double>(
				name: "ProdajnaCenaBezPDV",
				table: "KomercijalnoPrices",
				type: "double precision",
				nullable: false,
				oldClrType: typeof(decimal),
				oldType: "numeric"
			);

			migrationBuilder.AlterColumn<double>(
				name: "NabavnaCenaBezPDV",
				table: "KomercijalnoPrices",
				type: "double precision",
				nullable: false,
				oldClrType: typeof(decimal),
				oldType: "numeric"
			);
		}
	}
}
