using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class AddedUsersAuditFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Carriers_CarrierBankingDetailsId",
                table: "Carriers",
                column: "CarrierBankingDetailsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carriers_CarrierBankingDetails_CarrierBankingDetailsId",
                table: "Carriers",
                column: "CarrierBankingDetailsId",
                principalTable: "CarrierBankingDetails",
                principalColumn: "CarrierBankingDetailsId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriers_CarrierBankingDetails_CarrierBankingDetailsId",
                table: "Carriers");

            migrationBuilder.DropIndex(
                name: "IX_Carriers_CarrierBankingDetailsId",
                table: "Carriers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");
        }
    }
}
