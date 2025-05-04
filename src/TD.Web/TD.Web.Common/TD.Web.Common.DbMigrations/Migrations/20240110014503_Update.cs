using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class Update : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Settings",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Settings",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoWebProductLinks",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoWebProductLinks",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoPrices",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoPrices",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.CreateIndex(
				name: "IX_Settings_Key",
				table: "Settings",
				column: "Key",
				unique: true
			);

			migrationBuilder.CreateIndex(
				name: "IX_KomercijalnoWebProductLinks_RobaId_WebId",
				table: "KomercijalnoWebProductLinks",
				columns: new[] { "RobaId", "WebId" },
				unique: true
			);

			migrationBuilder.CreateIndex(
				name: "IX_KomercijalnoPrices_RobaId",
				table: "KomercijalnoPrices",
				column: "RobaId",
				unique: true
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(name: "IX_Settings_Key", table: "Settings");

			migrationBuilder.DropIndex(
				name: "IX_KomercijalnoWebProductLinks_RobaId_WebId",
				table: "KomercijalnoWebProductLinks"
			);

			migrationBuilder.DropIndex(
				name: "IX_KomercijalnoPrices_RobaId",
				table: "KomercijalnoPrices"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Settings",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Settings",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoWebProductLinks",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoWebProductLinks",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoPrices",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoPrices",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);
		}
	}
}
