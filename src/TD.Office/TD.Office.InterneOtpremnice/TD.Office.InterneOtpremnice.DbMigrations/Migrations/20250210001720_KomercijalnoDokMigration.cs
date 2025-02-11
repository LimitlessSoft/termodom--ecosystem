using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.InterneOtpremnice.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class KomercijalnoDokMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KomercijalnoBrDok",
                table: "InterneOtpremnice",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KomercijalnoVrDok",
                table: "InterneOtpremnice",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KomercijalnoBrDok",
                table: "InterneOtpremnice");

            migrationBuilder.DropColumn(
                name: "KomercijalnoVrDok",
                table: "InterneOtpremnice");
        }
    }
}
