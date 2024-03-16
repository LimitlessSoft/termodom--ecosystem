using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class UserReferentRelationCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Referent",
                table: "Users",
                newName: "ReferentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferentId",
                table: "Users",
                column: "ReferentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ReferentId",
                table: "Users",
                column: "ReferentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ReferentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ReferentId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ReferentId",
                table: "Users",
                newName: "Referent");
        }
    }
}
