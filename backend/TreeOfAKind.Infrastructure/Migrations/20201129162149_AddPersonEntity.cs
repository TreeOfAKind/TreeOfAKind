using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TreeOfAKind.Infrastructure.Migrations
{
    public partial class AddPersonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthUserId",
                schema: "trees",
                table: "UserProfiles",
                newName: "UserAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_AuthUserId",
                schema: "trees",
                table: "UserProfiles",
                newName: "IX_UserProfiles_UserAuthId");

            migrationBuilder.CreateTable(
                name: "People",
                schema: "trees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TreeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Trees_TreeId",
                        column: x => x.TreeId,
                        principalSchema: "trees",
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeUserProfile",
                schema: "trees",
                columns: table => new
                {
                    _ownedTreesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _treeOwnersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeUserProfile", x => new { x._ownedTreesId, x._treeOwnersId });
                    table.ForeignKey(
                        name: "FK_TreeUserProfile_Trees__ownedTreesId",
                        column: x => x._ownedTreesId,
                        principalSchema: "trees",
                        principalTable: "Trees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreeUserProfile_UserProfiles__treeOwnersId",
                        column: x => x._treeOwnersId,
                        principalSchema: "trees",
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_TreeId",
                schema: "trees",
                table: "People",
                column: "TreeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeUserProfile__treeOwnersId",
                schema: "trees",
                table: "TreeUserProfile",
                column: "_treeOwnersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People",
                schema: "trees");

            migrationBuilder.DropTable(
                name: "TreeUserProfile",
                schema: "trees");

            migrationBuilder.RenameColumn(
                name: "UserAuthId",
                schema: "trees",
                table: "UserProfiles",
                newName: "AuthUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_UserAuthId",
                schema: "trees",
                table: "UserProfiles",
                newName: "IX_UserProfiles_AuthUserId");
        }
    }
}
