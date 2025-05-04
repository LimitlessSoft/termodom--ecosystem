using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class UsersProductsGroupsMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ProductGroupEntityUserEntity",
				columns: table => new
				{
					ProductGroupsId = table.Column<long>(type: "bigint", nullable: false),
					UsersId = table.Column<long>(type: "bigint", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey(
						"PK_ProductGroupEntityUserEntity",
						x => new { x.ProductGroupsId, x.UsersId }
					);
					table.ForeignKey(
						name: "FK_ProductGroupEntityUserEntity_ProductGroups_ProductGroupsId",
						column: x => x.ProductGroupsId,
						principalTable: "ProductGroups",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
					table.ForeignKey(
						name: "FK_ProductGroupEntityUserEntity_Users_UsersId",
						column: x => x.UsersId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateIndex(
				name: "IX_ProductGroupEntityUserEntity_UsersId",
				table: "ProductGroupEntityUserEntity",
				column: "UsersId"
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "ProductGroupEntityUserEntity");
		}
	}
}
