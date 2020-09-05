using Microsoft.EntityFrameworkCore.Migrations;

namespace Booking.API.Migrations
{
    public partial class BookingPaymentsDeletion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingPayments_Bookings_BookingId",
                table: "BookingPayments");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "BookingPayments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingPayments_Bookings_BookingId",
                table: "BookingPayments",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingPayments_Bookings_BookingId",
                table: "BookingPayments");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "BookingPayments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BookingPayments_Bookings_BookingId",
                table: "BookingPayments",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
