using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class ProracunNUIDAddedMigratin : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "NUID",
				table: "Proracuni",
				type: "integer",
				nullable: false,
				defaultValue: 0
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "NUID", table: "Proracuni");
		}
	}
}
