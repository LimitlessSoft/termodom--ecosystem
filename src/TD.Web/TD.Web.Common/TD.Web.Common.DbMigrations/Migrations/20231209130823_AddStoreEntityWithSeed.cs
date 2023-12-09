using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TD.Web.Common.Contracts;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
    public partial class AddStoreEntityWithSeed : Migration
    {
        private readonly string UpFile = Path.Combine(Constants.DbMigrations.DbSeedsRoot, "004_StoresSeed.sql");
        private readonly string DownFile = Path.Combine(Constants.DbMigrations.DbSeedsDownRoot, "down_004_StoresSeed.sql");
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_FavoriteStoreId",
                table: "Users",
                column: "FavoriteStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Stores_FavoriteStoreId",
                table: "Users",
                column: "FavoriteStoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(File.ReadAllText(UpFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(DownFile));

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Stores_FavoriteStoreId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Users_FavoriteStoreId",
                table: "Users");
        }
    }
}
