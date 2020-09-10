using System.Collections.Generic;

namespace Hotel.API.Model.Dtos
{
    public class RoomForDetailsDto
    {
        public int Id { get; set; }
        public bool Available { get; set; }
        public string Description { get; set; }
        public int MaximumGuests { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public decimal RoomTypePrice { get; set; }
        public string RoomTypeDescription { get; set; }
        public string RoomTypeName { get; set; }
        public ICollection<RoomFacilitiesForDetailedDto> RoomFacilities { get; set; }
    }
}