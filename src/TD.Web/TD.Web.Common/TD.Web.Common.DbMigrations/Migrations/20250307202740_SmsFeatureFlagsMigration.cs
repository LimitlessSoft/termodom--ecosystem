using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class SmsFeatureFlagsMigration : Migration
    {
        private readonly string UpFile = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "003_SmsFeatureFlags.sql");
        private readonly string DownFile = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_003_SmsFeatureFlags.sql");
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UpFile);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DownFile);
        }
    }
}
