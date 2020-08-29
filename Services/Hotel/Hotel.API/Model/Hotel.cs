using System.Collections.Generic;
namespace Hotel.API.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}