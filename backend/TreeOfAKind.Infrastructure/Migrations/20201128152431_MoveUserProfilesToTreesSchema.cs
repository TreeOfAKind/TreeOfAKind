using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeOfAKind.Infrastructure.Migrations
{
    public partial class MoveUserProfilesToTreesSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfiles",
                newSchema: "trees");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                schema: "trees",
                table: "UserProfiles",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserProfiles",
                schema: "trees",
                newName: "UserProfiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "UserProfiles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }
    }
}
