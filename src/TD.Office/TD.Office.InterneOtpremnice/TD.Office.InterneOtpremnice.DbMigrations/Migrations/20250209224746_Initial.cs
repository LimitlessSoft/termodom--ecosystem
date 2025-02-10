using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.InterneOtpremnice.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InterneOtpremnice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PolazniMagacinId = table.Column<int>(type: "integer", nullable: false),
                    DestinacioniMagacinId = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterneOtpremnice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterneOtpremniceItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RobaId = table.Column<int>(type: "integer", nullable: false),
                    Kolicina = table.Column<decimal>(type: "numeric", nullable: false),
                    InternaOtpremnicaId = table.Column<long>(type: "bigint", nullable: false),
                    InternaOtpremnicaEntityId = table.Column<long>(type: "bigint", nullable: true),
                    InternaOtpremnicaEntityId1 = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterneOtpremniceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterneOtpremniceItems_InterneOtpremnice_InternaOtpremnicaE~",
                        column: x => x.InternaOtpremnicaEntityId,
                        principalTable: "InterneOtpremnice",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterneOtpremniceItems_InterneOtpremnice_InternaOtpremnicaId",
                        column: x => x.InternaOtpremnicaId,
                        principalTable: "InterneOtpremnice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterneOtpremniceItems_InterneOtpremnice_InternaOtpremnica~1",
                        column: x => x.InternaOtpremnicaEntityId1,
                        principalTable: "InterneOtpremnice",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterneOtpremniceItems_InternaOtpremnicaEntityId",
                table: "InterneOtpremniceItems",
                column: "InternaOtpremnicaEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InterneOtpremniceItems_InternaOtpremnicaEntityId1",
                table: "InterneOtpremniceItems",
                column: "InternaOtpremnicaEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_InterneOtpremniceItems_InternaOtpremnicaId",
                table: "InterneOtpremniceItems",
                column: "InternaOtpremnicaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterneOtpremniceItems");

            migrationBuilder.DropTable(
                name: "InterneOtpremnice");
        }
    }
}
