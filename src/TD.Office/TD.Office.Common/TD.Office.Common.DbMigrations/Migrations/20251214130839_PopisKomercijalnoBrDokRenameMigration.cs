using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class PopisKomercijalnoBrDokRenameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KomcijalnoBrDok",
                table: "Popisi",
                newName: "KomercijalnoBrDok");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KomercijalnoBrDok",
                table: "Popisi",
                newName: "KomcijalnoBrDok");
        }
    }
}
