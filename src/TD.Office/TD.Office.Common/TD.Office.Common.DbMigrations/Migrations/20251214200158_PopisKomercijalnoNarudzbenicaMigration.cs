using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class PopisKomercijalnoNarudzbenicaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KomercijalnoBrDok",
                table: "Popisi",
                newName: "KomercijalnoPopisBrDok");

            migrationBuilder.AddColumn<long>(
                name: "KomercijalnoNarudzbenicaBrDok",
                table: "Popisi",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KomercijalnoNarudzbenicaBrDok",
                table: "Popisi");

            migrationBuilder.RenameColumn(
                name: "KomercijalnoPopisBrDok",
                table: "Popisi",
                newName: "KomercijalnoBrDok");
        }
    }
}
