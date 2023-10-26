using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Admin.Contracts;

#nullable disable

namespace TD.Web.Admin.Api.Migrations
{
    public partial class DbSeedUnits : Migration
    {
        private readonly string UpFile = Path.Combine(Constants.DbSeedsRoot, "002_UnitsSeed.sql");
        private readonly string DownFile = Path.Combine(Constants.DbSeedsDownRoot, "down_002_UnitsSeed.sql");

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
