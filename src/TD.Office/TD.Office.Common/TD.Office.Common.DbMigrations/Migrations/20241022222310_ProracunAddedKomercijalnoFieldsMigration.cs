using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ProracunAddedKomercijalnoFieldsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KomercijalnoBrDok",
                table: "Proracuni",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KomercijalnoVrDok",
                table: "Proracuni",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KomercijalnoBrDok",
                table: "Proracuni");

            migrationBuilder.DropColumn(
                name: "KomercijalnoVrDok",
                table: "Proracuni");
        }
    }
}
