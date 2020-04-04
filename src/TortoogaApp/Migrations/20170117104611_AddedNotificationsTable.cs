using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TortoogaApp.Migrations
{
    public partial class AddedNotificationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    NotificationDateTime = table.Column<DateTime>(nullable: false),
                    NotificationDescription = table.Column<string>(nullable: true),
                    NotificationType = table.Column<int>(nullable: false),
                    ReadDateTime = table.Column<DateTime>(nullable: true),
                    RelevantNotificationRefId = table.Column<Guid>(nullable: false),
                    ReleventUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

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

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Carriers_CarrierBankingDetailsId",
                table: "Carriers");
        }
    }
}
