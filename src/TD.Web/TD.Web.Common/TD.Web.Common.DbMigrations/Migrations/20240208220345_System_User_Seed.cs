using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class System_User_Seed : Migration
    {
        private readonly string UpFile_007 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "007_SystemUserSeed.sql");
        private readonly string DownFile_007 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_007_SystemUserSeed.sql");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile_007));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile_007));
        }
    }
}
