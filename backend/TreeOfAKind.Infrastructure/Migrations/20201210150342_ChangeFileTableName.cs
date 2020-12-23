using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeOfAKind.Infrastructure.Migrations
{
    public partial class ChangeFileTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonsMainFiles_People_OwnerId",
                schema: "trees",
                table: "PersonsMainFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonsMainFiles",
                schema: "trees",
                table: "PersonsMainFiles");

            migrationBuilder.RenameTable(
                name: "PersonsMainFiles",
                schema: "trees",
                newName: "PersonsFiles",
                newSchema: "trees");

            migrationBuilder.RenameIndex(
                name: "IX_PersonsMainFiles_OwnerId",
                schema: "trees",
                table: "PersonsFiles",
                newName: "IX_PersonsFiles_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonsFiles",
                schema: "trees",
                table: "PersonsFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonsFiles_People_OwnerId",
                schema: "trees",
                table: "PersonsFiles",
                column: "OwnerId",
                principalSchema: "trees",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonsFiles_People_OwnerId",
                schema: "trees",
                table: "PersonsFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonsFiles",
                schema: "trees",
                table: "PersonsFiles");

            migrationBuilder.RenameTable(
                name: "PersonsFiles",
                schema: "trees",
                newName: "PersonsMainFiles",
                newSchema: "trees");

            migrationBuilder.RenameIndex(
                name: "IX_PersonsFiles_OwnerId",
                schema: "trees",
                table: "PersonsMainFiles",
                newName: "IX_PersonsMainFiles_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonsMainFiles",
                schema: "trees",
                table: "PersonsMainFiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonsMainFiles_People_OwnerId",
                schema: "trees",
                table: "PersonsMainFiles",
                column: "OwnerId",
                principalSchema: "trees",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
