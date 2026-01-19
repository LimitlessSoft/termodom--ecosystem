using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddTipKorisnika : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TipKorisnikaId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TipoviKorisnika",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Boja = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviKorisnika", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TipKorisnikaId",
                table: "Users",
                column: "TipKorisnikaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TipoviKorisnika_TipKorisnikaId",
                table: "Users",
                column: "TipKorisnikaId",
                principalTable: "TipoviKorisnika",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Seed default user types
            migrationBuilder.InsertData(
                table: "TipoviKorisnika",
                columns: new[] { "Id", "Naziv", "Boja", "IsActive", "CreatedAt", "CreatedBy" },
                values: new object[,]
                {
                    { 1L, "Korisnik", "#4CAF50", true, DateTime.UtcNow, 0L },
                    { 2L, "Magacioner", "#2196F3", true, DateTime.UtcNow, 0L }
                });

            // Set all existing users to "Korisnik" type (Id = 1)
            migrationBuilder.Sql("UPDATE \"Users\" SET \"TipKorisnikaId\" = 1 WHERE \"TipKorisnikaId\" IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reset user type references before dropping the table
            migrationBuilder.Sql("UPDATE \"Users\" SET \"TipKorisnikaId\" = NULL;");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_TipoviKorisnika_TipKorisnikaId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TipoviKorisnika");

            migrationBuilder.DropIndex(
                name: "IX_Users_TipKorisnikaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TipKorisnikaId",
                table: "Users");
        }
    }
}
