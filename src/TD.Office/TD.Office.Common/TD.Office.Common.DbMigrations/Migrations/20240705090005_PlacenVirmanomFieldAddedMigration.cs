using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class PlacenVirmanomFieldAddedMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "PlacenVirmanom",
				table: "NaloziZaPrevoz",
				type: "boolean",
				nullable: false,
				defaultValue: false
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "PlacenVirmanom", table: "NaloziZaPrevoz");
		}
	}
}
