using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	public partial class NalogZaPrevozImplementMigration : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "NaloziZaPrevoz",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					Mobilni = table.Column<string>(type: "text", nullable: false),
					CenaPrevozaBezPdv = table.Column<decimal>(type: "numeric", nullable: false),
					MiNaplatiliKupcuBezPdv = table.Column<decimal>(
						type: "numeric",
						nullable: false
					),
					Note = table.Column<string>(type: "text", nullable: false),
					Address = table.Column<string>(type: "text", nullable: false),
					VrDok = table.Column<int>(type: "integer", nullable: false),
					BrDok = table.Column<int>(type: "integer", nullable: false),
					Date = table.Column<DateTime>(type: "timestamp", nullable: false),
					StoreId = table.Column<int>(type: "integer", nullable: false),
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
					table.PrimaryKey("PK_NaloziZaPrevoz", x => x.Id);
				}
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "NaloziZaPrevoz");
		}
	}
}
