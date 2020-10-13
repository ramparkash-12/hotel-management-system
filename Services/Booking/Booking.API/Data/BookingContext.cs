using Microsoft.EntityFrameworkCore;
using Booking.API.Model;
using Booking.API.Idempotency;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.API.Data
{
  public class BookingContext : DbContext
  {
    public BookingContext(DbContextOptions<BookingContext> options) : base(options)
    {
    }

    public DbSet<Model.Booking> Bookings { get; set; }
    public DbSet<BookingPayment> BookingPayments { get; set; }
    public DbSet<EventRequest> EventRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Model.Booking>()
        .HasMany(b => b.BookingPayments)
        .WithOne(bp => bp.Booking)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

        builder.ApplyConfiguration(new EventRequestEntityTypeConfiguration());
    }

    public class EventRequestEntityTypeConfiguration : IEntityTypeConfiguration<EventRequest>
    {
      public void Configure(EntityTypeBuilder<EventRequest> requestConfiguration)
      {
        requestConfiguration.ToTable("EventRequests");
        requestConfiguration.HasKey(cr => cr.Id);
        requestConfiguration.Property(cr => cr.Name).IsRequired();
        requestConfiguration.Property(cr => cr.Time).IsRequired();
      }
    }
  }
}