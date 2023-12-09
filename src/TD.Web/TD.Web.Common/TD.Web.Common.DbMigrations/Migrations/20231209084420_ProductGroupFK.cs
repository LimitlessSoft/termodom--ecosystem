using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class ProductGroupFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ParentGroupId",
                table: "ProductGroups",
                column: "ParentGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroups_ProductGroups_ParentGroupId",
                table: "ProductGroups",
                column: "ParentGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroups_ProductGroups_ParentGroupId",
                table: "ProductGroups");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ParentGroupId",
                table: "ProductGroups");
        }
    }
}
