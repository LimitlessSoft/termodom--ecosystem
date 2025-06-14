using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ProductPriceGroupTrackUserLevelMigrationPartThree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackUserLevel",
                table: "ProductPriceGroups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TrackUserLevel",
                table: "ProductPriceGroups",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
