using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Hotel.API.Model
{
    public class Images
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Size { get; set; }
        public string Extension { get; set; }
        public string URI { get; set; }
        public int ImageType { get; set; }
        //** transaction id can be from room, hotel....
        public int TransactionId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }

    }

    enum ImageType : int
    {
        HotelImage = 1,
        RoomImage = 2
    }
}