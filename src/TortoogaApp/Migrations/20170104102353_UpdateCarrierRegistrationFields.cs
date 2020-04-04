using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class UpdateCarrierRegistrationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CarrierBankingDetailsId",
                table: "Carriers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CarrierBankingDetails",
                columns: table => new
                {
                    CarrierBankingDetailsId = table.Column<Guid>(nullable: false),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    BankIdentificationNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrierBankingDetails", x => x.CarrierBankingDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "CarrierRegistrations",
                columns: table => new
                {
                    CarrierRegistrationGuid = table.Column<Guid>(nullable: false),
                    BusinessName = table.Column<string>(nullable: true),
                    Completed = table.Column<bool>(nullable: false),
                    ContactPerson = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MCDotNumber = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TempPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrierRegistrations", x => x.CarrierRegistrationGuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrierBankingDetails");

            migrationBuilder.DropTable(
                name: "CarrierRegistrations");

            migrationBuilder.DropColumn(
                name: "CarrierBankingDetailsId",
                table: "Carriers");
        }
    }
}
