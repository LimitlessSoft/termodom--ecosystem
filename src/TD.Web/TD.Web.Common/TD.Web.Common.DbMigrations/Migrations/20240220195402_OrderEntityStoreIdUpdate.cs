using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class OrderEntityStoreIdUpdate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<short>(
				name: "StoreId",
				table: "Orders",
				type: "smallint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "StoreId",
				table: "Orders",
				type: "integer",
				nullable: false,
				oldClrType: typeof(short),
				oldType: "smallint"
			);
		}
	}
}
