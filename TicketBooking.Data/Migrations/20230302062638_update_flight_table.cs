using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBooking.Data.Migrations
{
    public partial class update_flight_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BusinessPrice",
                table: "Flight",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EconomyPrice",
                table: "Flight",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessPrice",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "EconomyPrice",
                table: "Flight");
        }
    }
}
