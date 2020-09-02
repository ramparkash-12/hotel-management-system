using System.Collections.Generic;
using Hotel.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Hotel.API.Data
{
  public static class ModelBuilderExtensions
  {
    public static void Seed(this ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Facility>().HasData(
          new List<Facility>()
            {
                new Facility() { Id = 19,  Name = "Iron/Hair dryer"},
                new Facility() { Id = 20, Name = "Pets Allowed"}
                /*new Facility() { Id = 21,  Name = "Cards Accepted"},
                new Facility() { Id = 22, Name = "Private bathroom"}, 
                new Facility() { Id = 23, Name = "Air Conditioner"},*/            
            }
      );

      modelBuilder.Entity<RoomType>().HasData(
          new List<RoomType>()
            {
                new RoomType() { Id = 4,  Name = "Sea View", Description = "Sea View Room", price = 1700}      
            }
      );
    }
  }
}