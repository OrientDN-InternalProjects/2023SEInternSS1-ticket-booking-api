using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBooking.Data.Migrations
{
    public partial class restructureDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ExtraService_ExtraBaggageId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingSeat_Seat_SeatId",
                table: "BookingSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Aircraft_AirCraftId",
                table: "Seat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatClass_SeatClassId",
                table: "Seat");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ExtraBaggageId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ExtraBaggageId",
                table: "Booking");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingSeat_Seat_SeatId",
                table: "BookingSeat",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Aircraft_AirCraftId",
                table: "Seat",
                column: "AirCraftId",
                principalTable: "Aircraft",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatClass_SeatClassId",
                table: "Seat",
                column: "SeatClassId",
                principalTable: "SeatClass",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingSeat_Seat_SeatId",
                table: "BookingSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Aircraft_AirCraftId",
                table: "Seat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_SeatClass_SeatClassId",
                table: "Seat");

            migrationBuilder.AddColumn<Guid>(
                name: "ExtraBaggageId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ExtraBaggageId",
                table: "Booking",
                column: "ExtraBaggageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ExtraService_ExtraBaggageId",
                table: "Booking",
                column: "ExtraBaggageId",
                principalTable: "ExtraService",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingSeat_Seat_SeatId",
                table: "BookingSeat",
                column: "SeatId",
                principalTable: "Seat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Aircraft_AirCraftId",
                table: "Seat",
                column: "AirCraftId",
                principalTable: "Aircraft",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_SeatClass_SeatClassId",
                table: "Seat",
                column: "SeatClassId",
                principalTable: "SeatClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
