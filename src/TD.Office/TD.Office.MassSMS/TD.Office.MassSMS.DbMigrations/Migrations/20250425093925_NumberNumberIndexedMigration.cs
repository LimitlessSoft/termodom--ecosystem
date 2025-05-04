using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.MassSMS.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class NumberNumberIndexedMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_Numbers_Number",
				table: "Numbers",
				column: "Number"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(name: "IX_Numbers_Number", table: "Numbers");
		}
	}
}
