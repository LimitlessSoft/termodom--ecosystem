using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class ProductGroupsSeed : Migration
    {
        private readonly string UpFile_005 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "006_ProductGroupsSeed.sql");
        private readonly string DownFile_005 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_006_ProductGroupsSeed.sql");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile_005));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile_005));
        }
    }
}
