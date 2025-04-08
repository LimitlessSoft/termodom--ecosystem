using Microsoft.EntityFrameworkCore.Migrations;
using TD.Office.MassSMS.Contracts.Enums;

#nullable disable

namespace TD.Office.MassSMS.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class SettingSeedMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(
				"INSERT INTO \"Settings\" (\"Setting\", \"Value\", \"IsActive\", \"CreatedAt\", \"CreatedBy\") VALUES ("
					+ (int)Setting.GlobalState
					+ ", '"
					+ GlobalState.Initial.ToString()
					+ "', true, now(), 1);"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("TRUNCATE TABLE \"Settings\" RESTART IDENTITY CASCADE;");
		}
	}
}
