using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class SmsFeatureFlagsMigration : Migration
    {
        private readonly string UpFile = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "013_SmsFeatureFlags.sql");
        private readonly string DownFile = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_013_SmsFeatureFlags.sql");
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile));
        }
    }
}
