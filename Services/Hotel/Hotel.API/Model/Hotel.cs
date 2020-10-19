using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Hotel.API.Model
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool Status  { get; set; }
        public int Stars { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime FeaturedFrom { get; set; }
        public DateTime FeaturedTo { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Images> Images { get; set; }
    }
}