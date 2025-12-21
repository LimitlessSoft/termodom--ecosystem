using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class NalogZaPrevozStatusMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "NaloziZaPrevoz",
                type: "integer",
                nullable: false,
                defaultValueSql: "0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "NaloziZaPrevoz");
        }
    }
}
