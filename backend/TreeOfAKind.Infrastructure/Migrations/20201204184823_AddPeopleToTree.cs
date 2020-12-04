using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeOfAKind.Infrastructure.Migrations
{
    public partial class AddPeopleToTree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biography",
                schema: "trees",
                table: "People",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                schema: "trees",
                table: "People",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeathDate",
                schema: "trees",
                table: "People",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "trees",
                table: "People",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "trees",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "trees",
                table: "People",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "trees",
                table: "People",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TreeRelations",
                schema: "trees",
                columns: table => new
                {
                    From = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    To = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelationType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeRelations", x => new { x.From, x.To, x.RelationType });
                    table.ForeignKey(
                        name: "FK_TreeRelations_Trees_TreeId",
                        column: x => x.TreeId,
                        principalSchema: "trees",
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeRelations_TreeId",
                schema: "trees",
                table: "TreeRelations",
                column: "TreeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreeRelations",
                schema: "trees");

            migrationBuilder.DropColumn(
                name: "Biography",
                schema: "trees",
                table: "People");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                schema: "trees",
                table: "People");

            migrationBuilder.DropColumn(
                name: "DeathDate",
                schema: "trees",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "trees",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "trees",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "trees",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "trees",
                table: "People");
        }
    }
}
