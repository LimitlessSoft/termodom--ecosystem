using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class DataSeed : Migration
	{
		private readonly string UpFile_002 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsRoot,
			"002_UnitsSeed.sql"
		);
		private readonly string DownFile_002 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsDownRoot,
			"down_002_UnitsSeed.sql"
		);
		private readonly string UpFile_003 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsRoot,
			"003_CitiesSeed.sql"
		);
		private readonly string DownFile_003 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsDownRoot,
			"down_003_CitiesSeed.sql"
		);
		private readonly string UpFile_004 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsRoot,
			"004_StoresSeed.sql"
		);
		private readonly string DownFile_004 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsDownRoot,
			"down_004_StoresSeed.sql"
		);

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(UpFile_002));
			migrationBuilder.Sql(File.ReadAllText(UpFile_003));
			migrationBuilder.Sql(File.ReadAllText(UpFile_004));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(DownFile_002));
			migrationBuilder.Sql(File.ReadAllText(DownFile_003));
			migrationBuilder.Sql(File.ReadAllText(DownFile_004));
		}
	}
}
