using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class SettingsAlterMigration : Migration
    {
        private readonly string UpFile_012 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "012_SettingKeysSeed.sql");
        private readonly string DownFile_012 = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_012_SettingKeysSeed.sql");
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserEntityId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserEntityId",
                table: "Orders",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserEntityId",
                table: "Orders",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
            
            migrationBuilder.Sql(File.ReadAllText(UpFile_012));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile_012));
            
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserEntityId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Orders");
        }
    }
}
