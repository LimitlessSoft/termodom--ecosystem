using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.MassSMS.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class NumberIsBlacklistedIndexedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Numbers_IsBlacklisted",
                table: "Numbers",
                column: "IsBlacklisted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Numbers_IsBlacklisted",
                table: "Numbers");
        }
    }
}
