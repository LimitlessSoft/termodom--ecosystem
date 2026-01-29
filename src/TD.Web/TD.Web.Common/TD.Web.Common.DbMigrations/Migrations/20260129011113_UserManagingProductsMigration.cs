using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class UserManagingProductsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductEntityUserEntity",
                columns: table => new
                {
                    ManagingProductsId = table.Column<long>(type: "bigint", nullable: false),
                    ManagingUsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEntityUserEntity", x => new { x.ManagingProductsId, x.ManagingUsersId });
                    table.ForeignKey(
                        name: "FK_ProductEntityUserEntity_Products_ManagingProductsId",
                        column: x => x.ManagingProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductEntityUserEntity_Users_ManagingUsersId",
                        column: x => x.ManagingUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntityUserEntity_ManagingUsersId",
                table: "ProductEntityUserEntity",
                column: "ManagingUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductEntityUserEntity");
        }
    }
}
