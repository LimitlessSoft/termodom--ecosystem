using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class PaymentTypeKomercijalnoNUIDFieldAdded : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "KomercijalnoNUID",
				table: "PaymentTypes",
				type: "integer",
				nullable: false,
				defaultValue: 0
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "KomercijalnoNUID", table: "PaymentTypes");
		}
	}
}
