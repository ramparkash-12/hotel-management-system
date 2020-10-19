using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.API.Model.EntityConfigurations
{
    public class ImagesEntityTypeConfiguration: IEntityTypeConfiguration<Images>
  {
    public void Configure(EntityTypeBuilder<Images> images)
    {
      images.HasOne(i => i.Room)
      .WithMany(h => h.Images)
      .HasForeignKey(i => i.RoomImageId);

      images.HasOne(i => i.Hotel)
      .WithMany(h => h.Images)
      .HasForeignKey(i => i.HotelImageId);
    }
  }
}