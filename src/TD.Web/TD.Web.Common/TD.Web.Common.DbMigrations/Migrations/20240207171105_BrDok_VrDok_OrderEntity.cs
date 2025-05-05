using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class BrDok_VrDok_OrderEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "BrDok",
				table: "Orders",
				type: "integer",
				nullable: true
			);

			migrationBuilder.AddColumn<int>(
				name: "VrDok",
				table: "Orders",
				type: "integer",
				nullable: true
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "BrDok", table: "Orders");

			migrationBuilder.DropColumn(name: "VrDok", table: "Orders");
		}
	}
}
