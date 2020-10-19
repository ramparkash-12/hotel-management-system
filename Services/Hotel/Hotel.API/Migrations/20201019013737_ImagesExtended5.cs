using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel.API.Migrations
{
    public partial class ImagesExtended5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Rooms_RoomImageId",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "RoomImageId",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HotelImageId",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images",
                column: "HotelImageId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Rooms_RoomImageId",
                table: "Images",
                column: "RoomImageId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelImageId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Rooms_RoomImageId",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "RoomImageId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HotelImageId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
