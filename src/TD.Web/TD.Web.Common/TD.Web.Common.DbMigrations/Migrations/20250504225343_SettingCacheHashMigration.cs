using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class SettingCacheHashMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				$"INSERT INTO \"Settings\" (\"Key\", \"Value\", \"IsActive\", \"CreatedAt\", \"CreatedBy\") VALUES (5, '{Guid.NewGuid().ToString()}', true, CURRENT_TIMESTAMP, 0)"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql($"DELETE FROM \"Settings\" WHERE \"Key\" = 5");
		}
	}
}
