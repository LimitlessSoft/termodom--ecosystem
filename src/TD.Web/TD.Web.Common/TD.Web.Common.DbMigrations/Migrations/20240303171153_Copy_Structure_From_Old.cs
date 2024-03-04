using LSCore.Domain;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.DbMigrations.Helper;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class Copy_Structure_From_Old : Migration
    {
        private readonly string _sourceTableName = "kornik";
        private readonly string _destinationTableName = "old_users";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var config = LSCoreDomainConstants.Container.TryGetInstance<IConfigurationRoot>();
            MigrationHelper.ImportTableStructure(config, _sourceTableName, _destinationTableName);
            MigrationHelper.ImportData(config, _sourceTableName, _destinationTableName);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
