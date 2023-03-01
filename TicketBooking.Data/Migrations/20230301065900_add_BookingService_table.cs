using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBooking.Data.Migrations
{
    public partial class add_BookingService_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingList_ExtraService_ExtraServiceId",
                table: "BookingList");

            migrationBuilder.DropIndex(
                name: "IX_BookingList_ExtraServiceId",
                table: "BookingList");

            migrationBuilder.DropColumn(
                name: "ExtraServiceId",
                table: "BookingList");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingService_BookingList_BookingListId",
                        column: x => x.BookingListId,
                        principalTable: "BookingList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingService_ExtraService_ExtraServiceId",
                        column: x => x.ExtraServiceId,
                        principalTable: "ExtraService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingService_BookingListId",
                table: "BookingService",
                column: "BookingListId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingService_ExtraServiceId",
                table: "BookingService",
                column: "ExtraServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingService");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Booking");

            migrationBuilder.AddColumn<Guid>(
                name: "ExtraServiceId",
                table: "BookingList",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingList_ExtraServiceId",
                table: "BookingList",
                column: "ExtraServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingList_ExtraService_ExtraServiceId",
                table: "BookingList",
                column: "ExtraServiceId",
                principalTable: "ExtraService",
                principalColumn: "Id");
        }
    }
}
