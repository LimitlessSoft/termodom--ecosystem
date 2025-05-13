using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					Username = table.Column<string>(
						type: "character varying(64)",
						maxLength: 64,
						nullable: false
					),
					Password = table.Column<string>(
						type: "character varying(64)",
						maxLength: 64,
						nullable: false
					),
					Nickname = table.Column<string>(type: "text", nullable: false),
					Type = table.Column<int>(type: "integer", nullable: false),
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
					table.PrimaryKey("PK_Users", x => x.Id);
				}
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "Users");
		}
	}
}
