using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	public partial class AddedKomercijalnoPriceEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "KomercijalnoPrices",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					RobaId = table.Column<int>(type: "integer", nullable: false),
					NabavnaCenaBezPDV = table.Column<decimal>(type: "numeric", nullable: false),
					ProdajnaCenaBezPDV = table.Column<decimal>(type: "numeric", nullable: false),
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
					table.PrimaryKey("PK_KomercijalnoPrices", x => x.Id);
				}
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
			migrationBuilder.DropTable(name: "KomercijalnoPrices");
		}
	}
}
