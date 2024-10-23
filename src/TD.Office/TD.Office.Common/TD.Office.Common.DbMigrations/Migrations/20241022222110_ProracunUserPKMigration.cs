using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ProracunUserPKMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Proracuni",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Proracuni_UserId",
                table: "Proracuni",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proracuni_Users_UserId",
                table: "Proracuni",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proracuni_Users_UserId",
                table: "Proracuni");

            migrationBuilder.DropIndex(
                name: "IX_Proracuni_UserId",
                table: "Proracuni");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Proracuni");
        }
    }
}
