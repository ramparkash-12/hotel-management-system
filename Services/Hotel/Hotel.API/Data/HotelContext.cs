using Microsoft.EntityFrameworkCore;
using Hotel.API.Model;
using Microsoft.AspNetCore.Mvc;
using Hotel.API.Model.EntityConfigurations;

namespace Hotel.API.Data
{
  public class HotelContext : DbContext
  {
    public HotelContext(DbContextOptions<HotelContext> options) : base(options)
    {
    }

    public DbSet<Model.Hotel> Hotels { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Images> Images { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<RoomFacilities> RoomFacilities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new RoomFacilitiesEntityTypeConfiguration());
      builder.ApplyConfiguration(new ImagesEntityTypeConfiguration());
      builder.Seed();
      
      //base.OnModelCreating(builder);

    }
  }
}