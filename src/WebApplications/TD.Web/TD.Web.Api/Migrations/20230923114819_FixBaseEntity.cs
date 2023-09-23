using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Api.Migrations
{
    public partial class FixBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Users",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Units",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Units",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Units",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Units",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "Products",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Products",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Products",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "ProductGroups",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ProductGroups",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "ProductGroups",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ProductGroups",
                newName: "CreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Users",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Units",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Units",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Units",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Units",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Products",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Products",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ProductGroups",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ProductGroups",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ProductGroups",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ProductGroups",
                newName: "created_at");
        }
    }
}
