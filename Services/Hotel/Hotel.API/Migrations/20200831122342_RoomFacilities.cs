using Microsoft.EntityFrameworkCore.Migrations;

namespace Hotel.API.Migrations
{
    public partial class RoomFacilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Facilities_FacilitiesId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_FacilitiesId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "FacilitiesId",
                table: "Rooms");

            migrationBuilder.CreateTable(
                name: "RoomFacilities",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    FacilityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomFacilities", x => new { x.FacilityId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_RoomFacilities_Facilities_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomFacilities_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomFacilities_RoomId",
                table: "RoomFacilities",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomFacilities");

            migrationBuilder.AddColumn<int>(
                name: "FacilitiesId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_FacilitiesId",
                table: "Rooms",
                column: "FacilitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Facilities_FacilitiesId",
                table: "Rooms",
                column: "FacilitiesId",
                principalTable: "Facilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
