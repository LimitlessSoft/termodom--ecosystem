using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD.Web.Common.DbMigrations.Migrations
{
	public partial class InformationFix : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "OrderOneTimeInformations");

			migrationBuilder.CreateTable(
				name: "OrderOneTimeInformation",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					Name = table.Column<string>(type: "text", nullable: false),
					Mobile = table.Column<string>(type: "text", nullable: false),
					OrderId = table.Column<int>(type: "integer", nullable: false),
					IsActive = table.Column<bool>(
						type: "boolean",
						nullable: false,
						defaultValue: true
					),
					CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
					CreatedBy = table.Column<int>(type: "integer", nullable: false),
					UpdatedBy = table.Column<int>(type: "integer", nullable: true),
					UpdatedAt = table.Column<DateTime>(
						type: "timestamp without time zone",
						nullable: true
					)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderOneTimeInformation", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderOneTimeInformation_Orders_OrderId",
						column: x => x.OrderId,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateIndex(
				name: "IX_OrderOneTimeInformation_OrderId",
				table: "OrderOneTimeInformation",
				column: "OrderId",
				unique: true
			);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "OrderOneTimeInformation");

			migrationBuilder.CreateTable(
				name: "OrderOneTimeInformations",
				columns: table => new
				{
					Id = table
						.Column<int>(type: "integer", nullable: false)
						.Annotation(
							"Npgsql:ValueGenerationStrategy",
							NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
						),
					OrderId = table.Column<int>(type: "integer", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
					CreatedBy = table.Column<int>(type: "integer", nullable: false),
					IsActive = table.Column<bool>(
						type: "boolean",
						nullable: false,
						defaultValue: true
					),
					Mobile = table.Column<string>(type: "text", nullable: false),
					Name = table.Column<string>(type: "text", nullable: false),
					UpdatedAt = table.Column<DateTime>(
						type: "timestamp without time zone",
						nullable: true
					),
					UpdatedBy = table.Column<int>(type: "integer", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderOneTimeInformations", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderOneTimeInformations_Orders_OrderId",
						column: x => x.OrderId,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade
					);
				}
			);

			migrationBuilder.CreateIndex(
				name: "IX_OrderOneTimeInformations_OrderId",
				table: "OrderOneTimeInformations",
				column: "OrderId",
				unique: true
			);
		}
	}
}
