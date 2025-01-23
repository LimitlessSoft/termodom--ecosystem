using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.InterneOtpremnice.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class InternaOtpItemsFKMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InternaOtpremnicaItemEntity_InternaOtpremnicaId",
                table: "InternaOtpremnicaItemEntity",
                column: "InternaOtpremnicaId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternaOtpremnicaItemEntity_InterneOtpremnice_InternaOtprem~",
                table: "InternaOtpremnicaItemEntity",
                column: "InternaOtpremnicaId",
                principalTable: "InterneOtpremnice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternaOtpremnicaItemEntity_InterneOtpremnice_InternaOtprem~",
                table: "InternaOtpremnicaItemEntity");

            migrationBuilder.DropIndex(
                name: "IX_InternaOtpremnicaItemEntity_InternaOtpremnicaId",
                table: "InternaOtpremnicaItemEntity");
        }
    }
}
