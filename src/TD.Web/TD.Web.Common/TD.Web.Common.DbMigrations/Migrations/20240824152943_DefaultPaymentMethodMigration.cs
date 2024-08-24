using Microsoft.EntityFrameworkCore.Migrations;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class DefaultPaymentMethodMigration : Migration
    {
        private readonly string UpFile_002 = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "010_PaymentTypeExistingPopulation.sql");
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // if not exist, insert default payment type with id 0
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM ""PaymentTypes"" WHERE ""Id"" = 0) THEN
                        INSERT INTO ""PaymentTypes"" (""Id"", ""Name"", ""IsActive"", ""CreatedAt"", ""CreatedBy"")
                        VALUES (0, 'Default Payment Type', false, now(), 0);
                    END IF;
                END
                $$;
            ");
            migrationBuilder.AddColumn<long>(
                name: "DefaultPaymentTypeId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "PaymentTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DefaultPaymentTypeId",
                table: "Users",
                column: "DefaultPaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PaymentTypes_DefaultPaymentTypeId",
                table: "Users",
                column: "DefaultPaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_PaymentTypes_DefaultPaymentTypeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DefaultPaymentTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DefaultPaymentTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "PaymentTypes");
        }
    }
}
