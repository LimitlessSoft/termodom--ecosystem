using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.MassSMS.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class NumberIsNowUniqueMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Numbers_Number",
                table: "Numbers");

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_Number",
                table: "Numbers",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Numbers_Number",
                table: "Numbers");

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_Number",
                table: "Numbers",
                column: "Number");
        }
    }
}
