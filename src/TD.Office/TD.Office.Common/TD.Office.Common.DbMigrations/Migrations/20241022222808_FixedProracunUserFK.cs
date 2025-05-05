using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
	/// <inheritdoc />
	public partial class FixedProracunUserFK : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(name: "FK_Proracuni_Users_UserId", table: "Proracuni");

			migrationBuilder.DropIndex(name: "IX_Proracuni_UserId", table: "Proracuni");

			migrationBuilder.DropColumn(name: "UserId", table: "Proracuni");

			migrationBuilder.CreateIndex(
				name: "IX_Proracuni_CreatedBy",
				table: "Proracuni",
				column: "CreatedBy"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_Proracuni_Users_CreatedBy",
				table: "Proracuni",
				column: "CreatedBy",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Proracuni_Users_CreatedBy",
				table: "Proracuni"
			);

			migrationBuilder.DropIndex(name: "IX_Proracuni_CreatedBy", table: "Proracuni");

			migrationBuilder.AddColumn<long>(
				name: "UserId",
				table: "Proracuni",
				type: "bigint",
				nullable: false,
				defaultValue: 0L
			);

			migrationBuilder.CreateIndex(
				name: "IX_Proracuni_UserId",
				table: "Proracuni",
				column: "UserId"
			);

			migrationBuilder.AddForeignKey(
				name: "FK_Proracuni_Users_UserId",
				table: "Proracuni",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade
			);
		}
	}
}
