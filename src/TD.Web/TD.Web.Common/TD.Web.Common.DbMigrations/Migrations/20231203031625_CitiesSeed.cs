using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class CitiesSeed : Migration
    {
        private readonly string UpFile = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "003_CitiesSeed.sql");
        private readonly string DownFile = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_003_CitiesSeed.sql");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile));
        }
    }
}
