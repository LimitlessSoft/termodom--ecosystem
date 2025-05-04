using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class Add_Komercijalno_VrDok_BrDok : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "VrDok",
				table: "Orders",
				newName: "KomercijalnoVrDok"
			);

			migrationBuilder.RenameColumn(
				name: "BrDok",
				table: "Orders",
				newName: "KomercijalnoBrDok"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "KomercijalnoVrDok",
				table: "Orders",
				newName: "VrDok"
			);

			migrationBuilder.RenameColumn(
				name: "KomercijalnoBrDok",
				table: "Orders",
				newName: "BrDok"
			);
		}
	}
}
