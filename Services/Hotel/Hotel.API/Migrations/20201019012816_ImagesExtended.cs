using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel.API.Migrations
{
    public partial class ImagesExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Images",
                newName: "RoomImageId");

            migrationBuilder.AddColumn<int>(
                name: "HotelImageId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_HotelImageId",
                table: "Images",
                column: "HotelImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_RoomImageId",
                table: "Images",
                column: "RoomImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images",
                column: "HotelImageId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Rooms_RoomImageId",
                table: "Images",
                column: "RoomImageId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Rooms_RoomImageId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_HotelImageId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_RoomImageId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "HotelImageId",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "RoomImageId",
                table: "Images",
                newName: "TransactionId");
        }
    }
}
