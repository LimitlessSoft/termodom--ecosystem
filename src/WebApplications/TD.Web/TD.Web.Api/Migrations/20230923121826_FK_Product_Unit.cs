using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Web.Api.Migrations
{
    public partial class FK_Product_Unit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Units_UnitId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitId",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "updated_by",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "updated_by",
                table: "Products",
                type: "bigint",
                nullable: true);
        }
    }
}
