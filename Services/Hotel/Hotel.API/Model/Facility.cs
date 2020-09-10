using System.Collections.Generic;
namespace Hotel.API.Model
{
    public class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoomFacilities> RoomFacilities { get; set; }
    }
}