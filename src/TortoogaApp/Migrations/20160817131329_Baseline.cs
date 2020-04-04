using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class Baseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Carrier_CarrierGuid",
                table: "Listings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carrier",
                table: "Carrier");

            migrationBuilder.DropColumn(
                name: "CarrierName",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Carrier");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingGuid = table.Column<Guid>(nullable: false),
                    BookedHeight = table.Column<double>(nullable: false),
                    BookedLength = table.Column<double>(nullable: false),
                    BookedWidth = table.Column<double>(nullable: false),
                    BookingAmount = table.Column<decimal>(nullable: false),
                    CarrierGuid = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ListingGuid = table.Column<Guid>(nullable: false),
                    PaymentReferenceNumber = table.Column<string>(nullable: true),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingGuid);
                    table.ForeignKey(
                        name: "FK_Bookings_Listings_ListingGuid",
                        column: x => x.ListingGuid,
                        principalTable: "Listings",
                        principalColumn: "ListingGuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "CarrierGuid",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropOffAddress",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Listings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PickUpAddress",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Carrier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Carrier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Carrier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Carrier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Carrier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Carrier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CarrierGuid",
                table: "AspNetUsers",
                column: "CarrierGuid");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Listings",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carriers",
                table: "Carrier",
                column: "CarrierGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ListingGuid",
                table: "Bookings",
                column: "ListingGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Carriers_CarrierGuid",
                table: "Listings",
                column: "CarrierGuid",
                principalTable: "Carrier",
                principalColumn: "CarrierGuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Carriers_CarrierGuid",
                table: "AspNetUsers",
                column: "CarrierGuid",
                principalTable: "Carrier",
                principalColumn: "CarrierGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.RenameTable(
                name: "Carrier",
                newName: "Carriers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Carriers_CarrierGuid",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Carriers_CarrierGuid",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CarrierGuid",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carriers",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "CarrierGuid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DropOffAddress",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PickUpAddress",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Carriers");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "CarrierName",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressId",
                table: "Carriers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Listings",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carrier",
                table: "Carriers",
                column: "CarrierGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Carrier_CarrierGuid",
                table: "Listings",
                column: "CarrierGuid",
                principalTable: "Carriers",
                principalColumn: "CarrierGuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameTable(
                name: "Carriers",
                newName: "Carrier");
        }
    }
}
