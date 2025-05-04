using Microsoft.EntityFrameworkCore.Migrations;
using TD.Office.Common.Contracts;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class KomercijalnoIFinansijskoPoGodinamaStatusIsDefaultMigration : Migration
	{
		private readonly string UpFile_003 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsRoot,
			"003_KomercijalnoIFinansijskoPoGodinamaStatusSeed_01.sql"
		);

		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(UpFile_003));
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder) { }
	}
}
