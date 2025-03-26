using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.Contracts;
using TD.Web.Common.DbMigrations.Helper;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class ImportLegacyUsersLevelsData : Migration
	{
		private readonly string UpFile_002 = Path.Combine(
			LegacyConstants.DbMigrations.DbImportDataRoot,
			"002_MigrateProductPriceUsersLevelsData.sql"
		);
		private readonly string DownFile_002 = Path.Combine(
			LegacyConstants.DbMigrations.DbImportDataDownRoot,
			"down_002_MigrateProductPriceUsersLevelsData.sql"
		);
		private readonly string _sourceTableName = "user_cenovnik";
		private readonly string _destinationTableName = "old_user_cenovnik";

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// var config = LSCoreDomainConstants.Container.TryGetInstance<IConfigurationRoot>();
			// MigrationHelper.ImportTableStructure(config, _sourceTableName, _destinationTableName);
			// MigrationHelper.ImportData(config, _sourceTableName, _destinationTableName);

			// migrationBuilder.Sql(File.ReadAllText(UpFile_002));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// migrationBuilder.Sql(File.ReadAllText(DownFile_002));
		}
	}
}
