using Microsoft.EntityFrameworkCore.Migrations;
using TD.Office.Common.Contracts;


#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    public partial class Partners_Per_Years_Period_Seed : Migration
    {

        private readonly string UpFile_001 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "001_DefaultPartnerYearsSeed.sql");
        private readonly string DownFile_001 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_001_DefaultPartnerYearsSeed.sql");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile_001));
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile_001));
        }
    }
}
