using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class ProfessionEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "ProfessionId",
				table: "Users",
				type: "integer",
				nullable: true
			);

			migrationBuilder.CreateTable(
				name: "Professions",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					Name = table.Column<string>(
						type: "character varying(32)",
						maxLength: 32,
						nullable: false
					),
					IsActive = table.Column<bool>(
						type: "boolean",
						nullable: false,
						defaultValue: true
					),
					CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
					CreatedBy = table.Column<int>(type: "integer", nullable: false),
					UpdatedBy = table.Column<int>(type: "integer", nullable: true),
					UpdatedAt = table.Column<DateTime>(
						type: "timestamp without time zone",
						nullable: true
					)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Professions", x => x.Id);
				}
			);

			migrationBuilder.CreateIndex(
				name: "IX_Users_ProfessionId",
				table: "Users",
				column: "ProfessionId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_Users_Professions_ProfessionId",
				table: "Users",
				column: "ProfessionId",
				principalTable: "Professions",
				principalColumn: "Id"
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Users_Professions_ProfessionId",
				table: "Users"
			);

			migrationBuilder.DropTable(name: "Professions");

			migrationBuilder.DropIndex(name: "IX_Users_ProfessionId", table: "Users");

			migrationBuilder.DropColumn(name: "ProfessionId", table: "Users");
		}
	}
}
