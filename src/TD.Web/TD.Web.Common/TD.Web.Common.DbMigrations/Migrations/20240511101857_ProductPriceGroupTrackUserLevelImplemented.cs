using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class ProductPriceGroupTrackUserLevelImplemented : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "TrackUserLevel",
				table: "ProductPriceGroups",
				type: "boolean",
				nullable: false,
				defaultValue: false
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "TrackUserLevel", table: "ProductPriceGroups");
		}
	}
}
