using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class PaymentTypeMappingAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentTypeId",
                table: "Orders",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentTypes_PaymentTypeId",
                table: "Orders",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentTypes_PaymentTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentTypeId",
                table: "Orders");
        }
    }
}
