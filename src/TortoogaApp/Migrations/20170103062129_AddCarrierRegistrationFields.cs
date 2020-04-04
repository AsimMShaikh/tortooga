using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class AddCarrierRegistrationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyBio",
                table: "Carriers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "Carriers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MCDotNumber",
                table: "Carriers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyBio",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "MCDotNumber",
                table: "Carriers");
        }
    }
}
