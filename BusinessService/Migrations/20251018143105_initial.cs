using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreCategoryRelations",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCategoryRelations", x => new { x.StoreId, x.StoreCategoryId });
                    table.ForeignKey(
                        name: "FK_StoreCategoryRelations_StoreCategories_StoreCategoryId",
                        column: x => x.StoreCategoryId,
                        principalTable: "StoreCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreCategoryRelations_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreTagRelations",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTagRelations", x => new { x.StoreId, x.StoreTagId });
                    table.ForeignKey(
                        name: "FK_StoreTagRelations_StoreTags_StoreTagId",
                        column: x => x.StoreTagId,
                        principalTable: "StoreTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreTagRelations_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreCategoryRelations_StoreCategoryId",
                table: "StoreCategoryRelations",
                column: "StoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTagRelations_StoreTagId",
                table: "StoreTagRelations",
                column: "StoreTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreCategoryRelations");

            migrationBuilder.DropTable(
                name: "StoreTagRelations");

            migrationBuilder.DropTable(
                name: "StoreCategories");

            migrationBuilder.DropTable(
                name: "StoreTags");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
