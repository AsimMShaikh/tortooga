using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class AddCompanyLogotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "CarrierRegistrations");

            migrationBuilder.CreateTable(
                name: "CompanyLogos",
                columns: table => new
                {
                    ImageGuid = table.Column<Guid>(nullable: false),
                    CarrierGuid = table.Column<Guid>(nullable: false),
                    ContentType = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    ImageUri = table.Column<string>(nullable: true),
                    bytes = table.Column<byte[]>(nullable: true),
                    isDeleted = table.Column<bool>(nullable: false),
                    size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLogos", x => x.ImageGuid);
                    table.ForeignKey(
                        name: "FK_CompanyLogos_Carriers_CarrierGuid",
                        column: x => x.CarrierGuid,
                        principalTable: "Carriers",
                        principalColumn: "CarrierGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLogos_CarrierGuid",
                table: "CompanyLogos",
                column: "CarrierGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyLogos");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "CarrierRegistrations",
                nullable: false,
                defaultValue: false);
        }
    }
}
