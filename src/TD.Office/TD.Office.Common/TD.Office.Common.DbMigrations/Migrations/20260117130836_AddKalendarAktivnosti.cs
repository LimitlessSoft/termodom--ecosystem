using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddKalendarAktivnosti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoviOdsustva",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Redosled = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviOdsustva", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Odsustva",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TipOdsustvaId = table.Column<long>(type: "bigint", nullable: false),
                    DatumOd = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DatumDo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Komentar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odsustva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odsustva_TipoviOdsustva_TipOdsustvaId",
                        column: x => x.TipOdsustvaId,
                        principalTable: "TipoviOdsustva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odsustva_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Odsustva_TipOdsustvaId",
                table: "Odsustva",
                column: "TipOdsustvaId");

            migrationBuilder.CreateIndex(
                name: "IX_Odsustva_UserId",
                table: "Odsustva",
                column: "UserId");

            // Seed default absence types
            migrationBuilder.InsertData(
                table: "TipoviOdsustva",
                columns: new[] { "Id", "Naziv", "Redosled", "IsActive", "CreatedAt", "CreatedBy" },
                values: new object[,]
                {
                    { 1L, "Slava", 0, true, DateTime.UtcNow, 0L },
                    { 2L, "Odmor", 1, true, DateTime.UtcNow, 0L },
                    { 3L, "Sahrana", 2, true, DateTime.UtcNow, 0L },
                    { 4L, "Ostalo", 3, true, DateTime.UtcNow, 0L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Odsustva");

            migrationBuilder.DropTable(
                name: "TipoviOdsustva");
        }
    }
}
