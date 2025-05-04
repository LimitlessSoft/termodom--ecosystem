using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class Public_And_Admin_Comment : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "AdminComment",
				table: "Orders",
				type: "text",
				nullable: true
			);

			migrationBuilder.AddColumn<string>(
				name: "PublicComment",
				table: "Orders",
				type: "text",
				nullable: true
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(name: "AdminComment", table: "Orders");

			migrationBuilder.DropColumn(name: "PublicComment", table: "Orders");
		}
	}
}
