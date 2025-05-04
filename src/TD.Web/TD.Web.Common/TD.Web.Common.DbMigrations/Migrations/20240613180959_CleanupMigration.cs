using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class CleanupMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				"INSERT INTO \"PaymentTypes\" (\"Name\", \"IsActive\", \"CreatedAt\", \"CreatedBy\", \"KomercijalnoNUID\") VALUES('Gotovinom', true, CURRENT_TIMESTAMP, 0, 5);"
			);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Users",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ReferentId",
				table: "Users",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ProfessionId",
				table: "Users",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "FavoriteStoreId",
				table: "Users",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Users",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CityId",
				table: "Users",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Users",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Units",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Units",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Units",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Stores",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Stores",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Stores",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "StatisticsItems",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "StatisticsItems",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "StatisticsItems",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Settings",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Settings",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Settings",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Professions",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Professions",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Professions",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Products",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "UnitId",
				table: "Products",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "ProductPriceGroupId",
				table: "Products",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "PriceId",
				table: "Products",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Products",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "AlternateUnitId",
				table: "Products",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Products",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "ProductPrices",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ProductId",
				table: "ProductPrices",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "ProductPrices",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "ProductPrices",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "ProductPriceGroups",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "ProductPriceGroups",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "ProductPriceGroups",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UserId",
				table: "ProductPriceGroupLevel",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "ProductPriceGroupLevel",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ProductPriceGroupId",
				table: "ProductPriceGroupLevel",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "ProductPriceGroupLevel",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "ProductPriceGroupLevel",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "ProductGroups",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ParentGroupId",
				table: "ProductGroups",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "ProductGroups",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "ProductGroups",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "ProductsId",
				table: "ProductEntityProductGroupEntity",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "GroupsId",
				table: "ProductEntityProductGroupEntity",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "PaymentTypes",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "PaymentTypes",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "PaymentTypes",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Orders",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ReferentId",
				table: "Orders",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "PaymentTypeId",
				table: "Orders",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Orders",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Orders",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "OrderOneTimeInformation",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "OrderId",
				table: "OrderOneTimeInformation",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "OrderOneTimeInformation",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "OrderOneTimeInformation",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "OrderItems",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "ProductId",
				table: "OrderItems",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "OrderId",
				table: "OrderItems",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "OrderItems",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "OrderItems",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "WebId",
				table: "KomercijalnoWebProductLinks",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "KomercijalnoWebProductLinks",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "KomercijalnoWebProductLinks",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "KomercijalnoWebProductLinks",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "GlobalAlerts",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "GlobalAlerts",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "GlobalAlerts",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<long>(
				name: "UpdatedBy",
				table: "Cities",
				type: "bigint",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "integer",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Cities",
				type: "boolean",
				nullable: false,
				defaultValue: true,
				oldClrType: typeof(bool),
				oldType: "boolean"
			);

			migrationBuilder.AlterColumn<long>(
				name: "CreatedBy",
				table: "Cities",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Cities",
				type: "timestamp",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp without time zone"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "Cities",
					type: "bigint",
					nullable: false,
					oldClrType: typeof(int),
					oldType: "integer"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Users",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ReferentId",
				table: "Users",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ProfessionId",
				table: "Users",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "FavoriteStoreId",
				table: "Users",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Users",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CityId",
				table: "Users",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Users",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Units",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Units",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Units",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Stores",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Stores",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Stores",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "StatisticsItems",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "StatisticsItems",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "StatisticsItems",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Settings",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Settings",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Settings",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Professions",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Professions",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Professions",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Products",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "UnitId",
				table: "Products",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "ProductPriceGroupId",
				table: "Products",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "PriceId",
				table: "Products",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Products",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "AlternateUnitId",
				table: "Products",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Products",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "ProductPrices",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ProductId",
				table: "ProductPrices",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "ProductPrices",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "ProductPrices",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "ProductPriceGroups",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "ProductPriceGroups",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "ProductPriceGroups",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UserId",
				table: "ProductPriceGroupLevel",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "ProductPriceGroupLevel",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ProductPriceGroupId",
				table: "ProductPriceGroupLevel",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "ProductPriceGroupLevel",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "ProductPriceGroupLevel",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "ProductGroups",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ParentGroupId",
				table: "ProductGroups",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "ProductGroups",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "ProductGroups",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "ProductsId",
				table: "ProductEntityProductGroupEntity",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "GroupsId",
				table: "ProductEntityProductGroupEntity",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "PaymentTypes",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "PaymentTypes",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "PaymentTypes",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Orders",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ReferentId",
				table: "Orders",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "PaymentTypeId",
				table: "Orders",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Orders",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Orders",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "OrderOneTimeInformation",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "OrderId",
				table: "OrderOneTimeInformation",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "OrderOneTimeInformation",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "OrderOneTimeInformation",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "OrderItems",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "ProductId",
				table: "OrderItems",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "OrderId",
				table: "OrderItems",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "OrderItems",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "OrderItems",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "WebId",
				table: "KomercijalnoWebProductLinks",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "KomercijalnoWebProductLinks",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "KomercijalnoWebProductLinks",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "KomercijalnoWebProductLinks",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "GlobalAlerts",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "GlobalAlerts",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "GlobalAlerts",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);

			migrationBuilder.AlterColumn<int>(
				name: "UpdatedBy",
				table: "Cities",
				type: "integer",
				nullable: true,
				oldClrType: typeof(long),
				oldType: "bigint",
				oldNullable: true
			);

			migrationBuilder.AlterColumn<bool>(
				name: "IsActive",
				table: "Cities",
				type: "boolean",
				nullable: false,
				oldClrType: typeof(bool),
				oldType: "boolean",
				oldDefaultValue: true
			);

			migrationBuilder.AlterColumn<int>(
				name: "CreatedBy",
				table: "Cities",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder.AlterColumn<DateTime>(
				name: "CreatedAt",
				table: "Cities",
				type: "timestamp without time zone",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "timestamp"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "Cities",
					type: "integer",
					nullable: false,
					oldClrType: typeof(long),
					oldType: "bigint"
				)
				.Annotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				)
				.OldAnnotation(
					"Npgsql:ValueGenerationStrategy",
					NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
				);
		}
	}
}
