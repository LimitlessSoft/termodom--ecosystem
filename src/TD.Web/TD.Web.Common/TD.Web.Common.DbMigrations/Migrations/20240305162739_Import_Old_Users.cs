using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.Contracts;
using TD.Web.Common.DbMigrations.Helper;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class Import_Old_Users : Migration
	{
		private readonly string UpFile_001 = Path.Combine(
			LegacyConstants.DbMigrations.DbImportDataRoot,
			"001_MapUsersData.sql"
		);
		private readonly string DownFile_001 = Path.Combine(
			LegacyConstants.DbMigrations.DbImportDataDownRoot,
			"down_001_MapUsersData.sql"
		);
		private readonly string _sourceTableName = "korisnik";
		private readonly string _destinationTableName = "old_users";

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// var config = LSCoreDomainConstants.Container.TryGetInstance<IConfigurationRoot>();
			// MigrationHelper.ImportTableStructure(config, _sourceTableName, _destinationTableName);
			// MigrationHelper.ImportData(config, _sourceTableName, _destinationTableName);

			// migrationBuilder.Sql(File.ReadAllText(UpFile_001));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// migrationBuilder.Sql(File.ReadAllText(DownFile_001));
		}
	}
}
