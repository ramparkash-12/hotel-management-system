using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel.API.Migrations
{
    public partial class HotelModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Hotels");
        }
    }
}
