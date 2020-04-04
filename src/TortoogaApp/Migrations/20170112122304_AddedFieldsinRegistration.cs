using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class AddedFieldsinRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "CarrierRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResponseBy",
                table: "CarrierRegistrations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDate",
                table: "CarrierRegistrations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "CarrierRegistrations");

            migrationBuilder.DropColumn(
                name: "ResponseBy",
                table: "CarrierRegistrations");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "CarrierRegistrations");
        }
    }
}
