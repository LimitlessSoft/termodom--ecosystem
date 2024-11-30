using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Setting_Properties_Seed : Migration
    {
        private readonly string UpFile_012 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "012_SettingKeysSeed.sql");
        private readonly string DownFile_012 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_012_SettingKeysSeed.sql");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile_012));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile_012));
        }
    }
}
