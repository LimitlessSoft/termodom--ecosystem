using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class AddedGlobalAlertEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "GlobalAlerts",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					Application = table.Column<int>(type: "integer", nullable: false),
					Text = table.Column<string>(
						type: "character varying(256)",
						maxLength: 256,
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
					table.PrimaryKey("PK_GlobalAlerts", x => x.Id);
				}
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "GlobalAlerts");
		}
	}
}
