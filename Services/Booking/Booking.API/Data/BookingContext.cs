using Microsoft.EntityFrameworkCore;
using Booking.API.Model;

namespace Booking.API.Data
{
  public class BookingContext : DbContext
  {
    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {
    }

    public DbSet<Model.Booking> Bookings { get; set; }
    public DbSet<BookingPayment> BookingPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Model.Booking>()
        .HasMany(b => b.BookingPayments)
        .WithOne(bp => bp.Booking)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}