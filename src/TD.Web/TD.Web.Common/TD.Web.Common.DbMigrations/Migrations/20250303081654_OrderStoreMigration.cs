using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class OrderStoreMigration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<long>(
				name: "StoreId",
				table: "Orders",
				type: "bigint",
				nullable: false,
				oldClrType: typeof(short),
				oldType: "smallint"
			);

			migrationBuilder.CreateIndex(
				name: "IX_Orders_StoreId",
				table: "Orders",
				column: "StoreId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_Orders_Stores_StoreId",
				table: "Orders",
				column: "StoreId",
				principalTable: "Stores",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(name: "FK_Orders_Stores_StoreId", table: "Orders");

			migrationBuilder.DropIndex(name: "IX_Orders_StoreId", table: "Orders");

			migrationBuilder.AlterColumn<short>(
				name: "StoreId",
				table: "Orders",
				type: "smallint",
				nullable: false,
				oldClrType: typeof(long),
				oldType: "bigint"
			);
		}
	}
}
