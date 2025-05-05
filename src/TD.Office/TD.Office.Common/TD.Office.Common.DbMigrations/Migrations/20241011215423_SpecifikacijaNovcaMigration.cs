using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class SpecifikacijaNovcaMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "SpecifikacijeNovca",
				columns: table => new
				{
					Id = table
						.Column<long>(type: "bigint", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					MagacinId = table.Column<long>(type: "bigint", nullable: false),
					Datum = table.Column<DateTime>(type: "timestamp", nullable: false),
					Komentar = table.Column<string>(type: "text", nullable: true),
					Eur1Komada = table.Column<int>(type: "integer", nullable: false),
					Eur1Kurs = table.Column<double>(type: "double precision", nullable: false),
					Eur2Komada = table.Column<int>(type: "integer", nullable: false),
					Eur2Kurs = table.Column<double>(type: "double precision", nullable: false),
					Novcanica5000Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica2000Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica1000Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica500Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica200Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica100Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica50Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica20Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica10Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica5Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica2Komada = table.Column<int>(type: "integer", nullable: false),
					Novcanica1Komada = table.Column<int>(type: "integer", nullable: false),
					Kartice = table.Column<double>(type: "double precision", nullable: false),
					KarticeKomentar = table.Column<string>(type: "text", nullable: true),
					Cekovi = table.Column<double>(type: "double precision", nullable: false),
					CekoviKomentar = table.Column<string>(type: "text", nullable: true),
					Papiri = table.Column<double>(type: "double precision", nullable: false),
					PapiriKomentar = table.Column<string>(type: "text", nullable: true),
					Troskovi = table.Column<double>(type: "double precision", nullable: false),
					TroskoviKomentar = table.Column<string>(type: "text", nullable: true),
					Vozaci = table.Column<double>(type: "double precision", nullable: false),
					VozaciKomentar = table.Column<string>(type: "text", nullable: true),
					Sasa = table.Column<double>(type: "double precision", nullable: false),
					SasaKomentar = table.Column<string>(type: "text", nullable: true),
					IsActive = table.Column<bool>(
						type: "boolean",
						nullable: false,
						defaultValue: true
					),
					CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
					CreatedBy = table.Column<long>(type: "bigint", nullable: false),
					UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
					UpdatedAt = table.Column<DateTime>(
						type: "timestamp without time zone",
						nullable: true
					)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_SpecifikacijeNovca", x => x.Id);
				}
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "SpecifikacijeNovca");
		}
	}
}
