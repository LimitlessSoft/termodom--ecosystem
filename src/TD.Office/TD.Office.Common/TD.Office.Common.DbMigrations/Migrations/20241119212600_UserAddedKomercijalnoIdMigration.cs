using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class UserAddedKomercijalnoIdMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "KomercijalnoNalogId",
				table: "Users",
				type: "integer",
				nullable: true
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "KomercijalnoNalogId", table: "Users");
		}
	}
}
