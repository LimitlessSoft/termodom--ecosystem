using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TD.Office.Common.DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class OdsustvoApprovalWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OdobrenoAt",
                table: "Odsustva",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OdobrenoBy",
                table: "Odsustva",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RealizovanoKorisnik",
                table: "Odsustva",
                type: "boolean",
                nullable: false,
                defaultValueSql: "false");

            migrationBuilder.AddColumn<bool>(
                name: "RealizovanoOdobravac",
                table: "Odsustva",
                type: "boolean",
                nullable: false,
                defaultValueSql: "false");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Odsustva",
                type: "integer",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.CreateIndex(
                name: "IX_Odsustva_OdobrenoBy",
                table: "Odsustva",
                column: "OdobrenoBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Odsustva_Users_OdobrenoBy",
                table: "Odsustva",
                column: "OdobrenoBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odsustva_Users_OdobrenoBy",
                table: "Odsustva");

            migrationBuilder.DropIndex(
                name: "IX_Odsustva_OdobrenoBy",
                table: "Odsustva");

            migrationBuilder.DropColumn(
                name: "OdobrenoAt",
                table: "Odsustva");

            migrationBuilder.DropColumn(
                name: "OdobrenoBy",
                table: "Odsustva");

            migrationBuilder.DropColumn(
                name: "RealizovanoKorisnik",
                table: "Odsustva");

            migrationBuilder.DropColumn(
                name: "RealizovanoOdobravac",
                table: "Odsustva");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Odsustva");
        }
    }
}
