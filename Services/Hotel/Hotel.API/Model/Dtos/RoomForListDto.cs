namespace Hotel.API.Model.Dtos
{
    public class RoomForListDto
    {
        public int Id { get; set; }
        public bool Available { get; set; }
        public string Description { get; set; }
        public int MaximumGuests { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
    }
}