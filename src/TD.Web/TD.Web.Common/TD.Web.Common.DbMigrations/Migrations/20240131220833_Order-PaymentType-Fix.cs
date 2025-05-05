using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class OrderPaymentTypeFix : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "PaymentType",
				table: "Orders",
				newName: "PaymentTypeId"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "PaymentTypeId",
				table: "Orders",
				newName: "PaymentType"
			);
		}
	}
}
