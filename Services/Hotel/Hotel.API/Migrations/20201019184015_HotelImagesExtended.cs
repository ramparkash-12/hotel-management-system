using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel.API.Migrations
{
    public partial class HotelImagesExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images",
                column: "HotelImageId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images",
                column: "HotelImageId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
