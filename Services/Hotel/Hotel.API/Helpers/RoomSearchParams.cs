namespace Hotel.API.Helpers
{
    public class RoomSearchParams
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize 
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }

        public int RoomTypeId { get; set; }
        public int HotelId { get; set; }
        public bool isAvailable { get; set; }
    }
}