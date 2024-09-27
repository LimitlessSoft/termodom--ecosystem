using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class OrderDeliveryAddressMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Orders");
        }
    }
}
