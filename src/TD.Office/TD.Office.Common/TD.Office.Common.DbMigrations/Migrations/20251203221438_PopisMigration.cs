using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class PopisMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Popisi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MagacinId = table.Column<long>(type: "bigint", nullable: false),
                    Datum = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popisi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Popisi_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PopisItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PopisDokumentId = table.Column<long>(type: "bigint", nullable: false),
                    RobaId = table.Column<long>(type: "bigint", nullable: false),
                    PopisanaKolicina = table.Column<double>(type: "double precision", nullable: true),
                    NarucenaKolicina = table.Column<double>(type: "double precision", nullable: true),
                    PopisDokumentEntityId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopisItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopisItems_Popisi_PopisDokumentEntityId",
                        column: x => x.PopisDokumentEntityId,
                        principalTable: "Popisi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PopisItems_Popisi_PopisDokumentId",
                        column: x => x.PopisDokumentId,
                        principalTable: "Popisi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Popisi_CreatedBy",
                table: "Popisi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PopisItems_PopisDokumentEntityId",
                table: "PopisItems",
                column: "PopisDokumentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PopisItems_PopisDokumentId",
                table: "PopisItems",
                column: "PopisDokumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PopisItems");

            migrationBuilder.DropTable(
                name: "Popisi");
        }
    }
}
