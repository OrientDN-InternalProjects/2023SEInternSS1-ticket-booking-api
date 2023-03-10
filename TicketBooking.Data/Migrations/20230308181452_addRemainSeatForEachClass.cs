using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBooking.Data.Migrations
{
    public partial class addRemainSeatForEachClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "SeatClass",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RemainBusinessSeat",
                table: "Flight",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainEconomySeat",
                table: "Flight",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "SeatClass");

            migrationBuilder.DropColumn(
                name: "RemainBusinessSeat",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "RemainEconomySeat",
                table: "Flight");
        }
    }
}
