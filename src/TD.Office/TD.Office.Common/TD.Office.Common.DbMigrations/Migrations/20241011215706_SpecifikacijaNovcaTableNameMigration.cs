using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class SpecifikacijaNovcaTableNameMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropPrimaryKey(
				name: "PK_SpecifikacijeNovca",
				table: "SpecifikacijeNovca"
			);

			migrationBuilder.RenameTable(name: "SpecifikacijeNovca", newName: "SpecifikacijaNovca");

			migrationBuilder.AddPrimaryKey(
				name: "PK_SpecifikacijaNovca",
				table: "SpecifikacijaNovca",
				column: "Id"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropPrimaryKey(
				name: "PK_SpecifikacijaNovca",
				table: "SpecifikacijaNovca"
			);

			migrationBuilder.RenameTable(name: "SpecifikacijaNovca", newName: "SpecifikacijeNovca");

			migrationBuilder.AddPrimaryKey(
				name: "PK_SpecifikacijeNovca",
				table: "SpecifikacijeNovca",
				column: "Id"
			);
		}
	}
}
