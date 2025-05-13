using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class ProracunEmailAddedMigraiton : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Email",
				table: "Proracuni",
				type: "text",
				nullable: true
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "Email", table: "Proracuni");
		}
	}
}
