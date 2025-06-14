using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ProductPriceGroupTrackUserLevelMigrationPartFour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackUserLevelNew",
                table: "ProductPriceGroups",
                newName: "TrackUserLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrackUserLevel",
                table: "ProductPriceGroups",
                newName: "TrackUserLevelNew");
        }
    }
}
