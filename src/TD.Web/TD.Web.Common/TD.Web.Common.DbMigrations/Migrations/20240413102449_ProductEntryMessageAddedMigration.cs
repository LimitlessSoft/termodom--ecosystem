using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class ProductEntryMessageAddedMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "WelcomeMessage",
				table: "ProductGroups",
				type: "text",
				nullable: true
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "WelcomeMessage", table: "ProductGroups");
		}
	}
}
