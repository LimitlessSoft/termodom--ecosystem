using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class OrderEntityPropertiesChange : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "StoreId",
				table: "Orders",
				type: "integer",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "PaymentTypeId",
				table: "Orders",
				type: "integer",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<string>(
				name: "OneTimeHash",
				table: "Orders",
				type: "text",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "text",
				oldNullable: true
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "StoreId",
				table: "Orders",
				type: "integer",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<int>(
				name: "PaymentTypeId",
				table: "Orders",
				type: "integer",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<string>(
				name: "OneTimeHash",
				table: "Orders",
				type: "text",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "text"
			);
		}
	}
}
