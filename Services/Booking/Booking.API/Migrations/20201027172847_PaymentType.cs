using Microsoft.EntityFrameworkCore.Migrations;

namespace Booking.API.Migrations
{
    public partial class PaymentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "PaymentMethod",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "BookingPayments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_PaymentTypeId",
                table: "PaymentMethod",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingPayments_PaymentTypeId",
                table: "BookingPayments",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingPayments_PaymentType_PaymentTypeId",
                table: "BookingPayments",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethod_PaymentType_PaymentTypeId",
                table: "PaymentMethod",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingPayments_PaymentType_PaymentTypeId",
                table: "BookingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethod_PaymentType_PaymentTypeId",
                table: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_PaymentTypeId",
                table: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_BookingPayments_PaymentTypeId",
                table: "BookingPayments");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "BookingPayments");
        }
    }
}
