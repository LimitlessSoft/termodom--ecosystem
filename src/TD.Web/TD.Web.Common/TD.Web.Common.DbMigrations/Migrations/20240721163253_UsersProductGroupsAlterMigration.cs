using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class UsersProductGroupsAlterMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ProductGroupEntityUserEntity_ProductGroups_ProductGroupsId",
				table: "ProductGroupEntityUserEntity"
			);

			migrationBuilder.DropForeignKey(
				name: "FK_ProductGroupEntityUserEntity_Users_UsersId",
				table: "ProductGroupEntityUserEntity"
			);

			migrationBuilder.RenameColumn(
				name: "UsersId",
				table: "ProductGroupEntityUserEntity",
				newName: "ManagingUsersId"
			);

			migrationBuilder.RenameColumn(
				name: "ProductGroupsId",
				table: "ProductGroupEntityUserEntity",
				newName: "ManaginProductGroupsId"
			);

			migrationBuilder.RenameIndex(
				name: "IX_ProductGroupEntityUserEntity_UsersId",
				table: "ProductGroupEntityUserEntity",
				newName: "IX_ProductGroupEntityUserEntity_ManagingUsersId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_ProductGroupEntityUserEntity_ProductGroups_ManaginProductGr~",
				table: "ProductGroupEntityUserEntity",
				column: "ManaginProductGroupsId",
				principalTable: "ProductGroups",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);

			migrationBuilder.AddForeignKey(
				name: "FK_ProductGroupEntityUserEntity_Users_ManagingUsersId",
				table: "ProductGroupEntityUserEntity",
				column: "ManagingUsersId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ProductGroupEntityUserEntity_ProductGroups_ManaginProductGr~",
				table: "ProductGroupEntityUserEntity"
			);

			migrationBuilder.DropForeignKey(
				name: "FK_ProductGroupEntityUserEntity_Users_ManagingUsersId",
				table: "ProductGroupEntityUserEntity"
			);

			migrationBuilder.RenameColumn(
				name: "ManagingUsersId",
				table: "ProductGroupEntityUserEntity",
				newName: "UsersId"
			);

			migrationBuilder.RenameColumn(
				name: "ManaginProductGroupsId",
				table: "ProductGroupEntityUserEntity",
				newName: "ProductGroupsId"
			);

			migrationBuilder.RenameIndex(
				name: "IX_ProductGroupEntityUserEntity_ManagingUsersId",
				table: "ProductGroupEntityUserEntity",
				newName: "IX_ProductGroupEntityUserEntity_UsersId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_ProductGroupEntityUserEntity_ProductGroups_ProductGroupsId",
				table: "ProductGroupEntityUserEntity",
				column: "ProductGroupsId",
				principalTable: "ProductGroups",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);

			migrationBuilder.AddForeignKey(
				name: "FK_ProductGroupEntityUserEntity_Users_UsersId",
				table: "ProductGroupEntityUserEntity",
				column: "UsersId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);
		}
	}
}
