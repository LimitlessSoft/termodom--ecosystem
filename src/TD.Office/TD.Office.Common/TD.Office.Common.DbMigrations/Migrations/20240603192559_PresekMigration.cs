using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class PresekMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "UsloviFormiranjaWebcena",
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
				name: "UserId",
				table: "UserPermissions",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "integer"
			);

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "UserPermissions",
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

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "NaloziZaPrevoz",
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

			migrationBuilder
				.AlterColumn<long>(
					name: "Id",
					table: "KomercijalnoPrices",
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
			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "UsloviFormiranjaWebcena",
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
				name: "UserId",
				table: "UserPermissions",
				type: "integer",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "UserPermissions",
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

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "NaloziZaPrevoz",
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

			migrationBuilder
				.AlterColumn<int>(
					name: "Id",
					table: "KomercijalnoPrices",
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
