namespace Hotel.API.Helpers
{
    public class HotelSearchParams
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize 
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value;}
        }

        public string Name { get; set; }
        public string City { get; set; }
        public string dateRange { get; set; }
        public int Adults { get; set; }
    }
}