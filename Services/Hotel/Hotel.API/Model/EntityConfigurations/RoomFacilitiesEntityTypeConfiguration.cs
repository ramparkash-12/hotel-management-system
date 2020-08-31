using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.API.Model.EntityConfigurations
{
    public class RoomFacilitiesEntityTypeConfiguration : IEntityTypeConfiguration<RoomFacilities>
  {
    public void Configure(EntityTypeBuilder<RoomFacilities> roomFacilities)
    {
      roomFacilities.HasKey(rf => new { rf.FacilityId, rf.RoomId });

      roomFacilities.HasOne(rm => rm.Facility)
      .WithMany(f => f.RoomFacilities)
      .HasForeignKey(rm => rm.FacilityId)
      .IsRequired();

      roomFacilities.HasOne(rm => rm.Room)
      .WithMany(r => r.RoomFacilities)
      .HasForeignKey(rm => rm.RoomId)
      .IsRequired();
    }
  }
}