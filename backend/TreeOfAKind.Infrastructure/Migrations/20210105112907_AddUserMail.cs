using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeOfAKind.Infrastructure.Migrations
{
    public partial class AddUserMail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmailAddress",
                schema: "trees",
                table: "UserProfiles",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmailAddress",
                schema: "trees",
                table: "UserProfiles");
        }
    }
}
