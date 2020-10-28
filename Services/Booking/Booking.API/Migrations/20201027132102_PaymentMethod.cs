using Microsoft.EntityFrameworkCore.Migrations;

namespace Booking.API.Migrations
{
    public partial class PaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "BookingPayments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardHolderName = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    Expiry = table.Column<string>(nullable: true),
                    SecurityNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_PaymentMethodId",
                table: "BookingPayments",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingPayments_PaymentMethod_PaymentMethodId",
                table: "BookingPayments",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingPayments_PaymentMethod_PaymentMethodId",
                table: "BookingPayments");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_BookingPayments_PaymentMethodId",
                table: "BookingPayments");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "BookingPayments");
        }
    }
}
