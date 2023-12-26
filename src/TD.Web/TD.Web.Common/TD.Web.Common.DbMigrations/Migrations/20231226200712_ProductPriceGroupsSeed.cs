using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class ProductPriceGroupsSeed : Migration
    {
        private readonly string UpFile_005 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "005_ProductPriceGroupsSeed.sql");
        private readonly string DownFile_005 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_005_ProductPriceGroupsSeed.sql");
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
