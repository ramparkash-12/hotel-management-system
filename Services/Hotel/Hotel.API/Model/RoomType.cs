using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.API.Model
{
    public class RoomType
    {
        public int Id { get; set; }
         [Column(TypeName = "decimal(18,4)")]
        public decimal price { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}