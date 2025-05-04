using Microsoft.EntityFrameworkCore.Migrations;
using TD.Office.Common.Contracts;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class KomercijalnoIFinansijskoPoGodinamaSeed : Migration
	{
		private readonly string UpFile_003 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsRoot,
			"003_KomercijalnoIFinansijskoPoGodinamaStatusSeed.sql"
		);
		private readonly string DownFile_003 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsDownRoot,
			"down_003_KomercijalnoIFinansijskoPoGodinamaStatusSeed.sql"
		);

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(UpFile_003));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(DownFile_003));
		}
	}
}
