using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.InterneOtpremnice.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ItemsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinacioniMagacinId",
                table: "InterneOtpremnice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PolazniMagacinId",
                table: "InterneOtpremnice",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "InternaOtpremnicaItemEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RobaId = table.Column<int>(type: "integer", nullable: false),
                    Kolicina = table.Column<decimal>(type: "numeric", nullable: false),
                    InternaOtpremnicaId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternaOtpremnicaItemEntity", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternaOtpremnicaItemEntity");

            migrationBuilder.DropColumn(
                name: "DestinacioniMagacinId",
                table: "InterneOtpremnice");

            migrationBuilder.DropColumn(
                name: "PolazniMagacinId",
                table: "InterneOtpremnice");
        }
    }
}
