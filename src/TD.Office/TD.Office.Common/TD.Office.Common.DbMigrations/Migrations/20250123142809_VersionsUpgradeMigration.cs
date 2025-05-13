using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class VersionsUpgradeMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "UsloviFormiranjaWebcena",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "UsloviFormiranjaWebcena",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Users",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Users",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AddColumn<string>(
				name: "RefreshToken",
				table: "Users",
				type: "text",
				nullable: true
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "UserPermissions",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "UserPermissions",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "SpecifikacijaNovca",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "Datum",
				table: "SpecifikacijaNovca",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "SpecifikacijaNovca",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
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
				table: "ProracunItems",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "ProracunItems",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Proracuni",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Proracuni",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Notes",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Notes",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "NaloziZaPrevoz",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "NaloziZaPrevoz",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "MagacinCentri",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "MagacinCentri",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Logs",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Logs",
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

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoIFinansijskoPoGodinamaStatus",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoIFinansijskoPoGodinamaStatus",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoIFinansijskoPoGodinama",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoIFinansijskoPoGodinama",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "RefreshToken", table: "Users");

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "UsloviFormiranjaWebcena",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "UsloviFormiranjaWebcena",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Users",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Users",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "UserPermissions",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "UserPermissions",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "SpecifikacijaNovca",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "Datum",
				table: "SpecifikacijaNovca",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "SpecifikacijaNovca",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

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
				table: "ProracunItems",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "ProracunItems",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Proracuni",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Proracuni",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Notes",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Notes",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "NaloziZaPrevoz",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "NaloziZaPrevoz",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "MagacinCentri",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "MagacinCentri",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Logs",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Logs",
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

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoIFinansijskoPoGodinamaStatus",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoIFinansijskoPoGodinamaStatus",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "KomercijalnoIFinansijskoPoGodinama",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "KomercijalnoIFinansijskoPoGodinama",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);
		}
	}
}
