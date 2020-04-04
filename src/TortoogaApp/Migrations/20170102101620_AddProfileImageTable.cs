using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class AddProfileImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileImages",
                columns: table => new
                {
                    ImageGuid = table.Column<Guid>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    ImageUri = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    bytes = table.Column<byte[]>(nullable: true),
                    isDeleted = table.Column<bool>(nullable: false),
                    size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImages", x => x.ImageGuid);
                    table.ForeignKey(
                        name: "FK_ProfileImages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_UserId",
                table: "ProfileImages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileImages");
        }
    }
}
