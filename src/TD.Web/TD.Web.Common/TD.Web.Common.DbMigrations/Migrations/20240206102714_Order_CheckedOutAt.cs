using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class Order_CheckedOutAt : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "CheckedOutAt",
				table: "Orders",
				type: "timestamp without time zone",
				nullable: true
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "CheckedOutAt", table: "Orders");
		}
	}
}
