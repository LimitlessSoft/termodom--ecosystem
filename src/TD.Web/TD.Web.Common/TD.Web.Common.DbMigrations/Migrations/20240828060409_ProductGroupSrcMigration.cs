using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ProductGroupSrcMigration : Migration
    {
        private readonly string UpFile_002 = Path.Combine(
            Constants.DbMigrations.DbSeedsRoot,
            "011_ProductGroupSrcSeed.sql"
        );

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Src",
                table: "ProductGroups",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.Sql(File.ReadAllText(UpFile_002));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Src", table: "ProductGroups");
        }
    }
}
