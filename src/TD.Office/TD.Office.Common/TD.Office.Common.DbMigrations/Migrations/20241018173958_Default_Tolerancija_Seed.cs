using Microsoft.EntityFrameworkCore.Migrations;
using TD.Office.Common.Contracts;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Default_Tolerancija_Seed : Migration
    {

        private readonly string UpFile_002 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "002_DefaultTolerancijaSeed.sql");
        private readonly string DownFile_002 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_002_DefaultTolerancijaSeed.sql");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(UpFile_002));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile_002));
        }
    }
}
