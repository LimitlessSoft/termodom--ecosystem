using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class UserMaxRabatAddedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxRabatMPDokumenti",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 5m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxRabatVPDokumenti",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 5m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxRabatMPDokumenti",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaxRabatVPDokumenti",
                table: "Users");
        }
    }
}
