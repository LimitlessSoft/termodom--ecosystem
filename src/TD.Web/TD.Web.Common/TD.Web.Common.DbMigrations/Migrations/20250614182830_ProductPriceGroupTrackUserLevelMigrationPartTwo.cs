using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class ProductPriceGroupTrackUserLevelMigrationPartTwo : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				"UPDATE \"ProductPriceGroups\" SET \"TrackUserLevelNew\" = 0 WHERE \"TrackUserLevel\" = true; UPDATE \"ProductPriceGroups\" SET \"TrackUserLevelNew\" = 1 WHERE \"TrackUserLevel\" = false;"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				"UPDATE \"ProductPriceGroups\" SET \"TrackUserLevel\" = true WHERE \"TrackUserLevelNew\" = 0; UPDATE \"ProductPriceGroups\" SET \"TrackUserLevel\" = false WHERE \"TrackUserLevelNew\" = 1;"
			);
		}
	}
}
