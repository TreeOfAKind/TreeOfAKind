using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeOfAKind.Infrastructure.Migrations
{
    public partial class AddPersonFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PersonsMainFiles",
                schema: "trees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonsMainFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonsMainFiles_People_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "trees",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonsMainPhotos",
                schema: "trees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileUri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonsMainPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonsMainPhotos_People_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "trees",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonsMainFiles_OwnerId",
                schema: "trees",
                table: "PersonsMainFiles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonsMainPhotos_OwnerId",
                schema: "trees",
                table: "PersonsMainPhotos",
                column: "OwnerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonsMainFiles",
                schema: "trees");

            migrationBuilder.DropTable(
                name: "PersonsMainPhotos",
                schema: "trees");
        }
    }
}
