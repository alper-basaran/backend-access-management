using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Services.Access.Infra.Data.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    PermissionSubject = table.Column<int>(nullable: false),
                    PermissionLevel = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ObjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Sites",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Locks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SiteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Locks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_Locks_tbl_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "tbl_Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Locks_SiteId",
                table: "tbl_Locks",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Locks");

            migrationBuilder.DropTable(
                name: "tbl_Permissions");

            migrationBuilder.DropTable(
                name: "tbl_Sites");
        }
    }
}
