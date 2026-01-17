using Microsoft.EntityFrameworkCore.Migrations;
using TD.Office.Common.Contracts;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class UserKorisnikPermissionSeedMigration : Migration
	{
		private readonly string UpFile_004 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsRoot,
			"004_KorisniciListReadPermissionSeed.sql"
		);
		private readonly string DownFile_004 = Path.Combine(
			LegacyConstants.DbMigrations.DbSeedsDownRoot,
			"down_004_KorisniciListReadPermissionSeed.sql"
		);

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(UpFile_004));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(File.ReadAllText(DownFile_004));
		}
	}
}
