using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.API.Models;

namespace Identity.API.Models.EntityConfigurations
{
  public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
  {
    public void Configure(EntityTypeBuilder<Identity.API.Models.UserRole> userRole)
    {
      userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

      userRole.HasOne(ur => ur.Role)
      .WithMany(r => r.userRoles)
      .HasForeignKey(ur => ur.RoleId)
      .IsRequired();

      userRole.HasOne(ur => ur.User)
      .WithMany(r => r.userRoles)
      .HasForeignKey(ur => ur.UserId)
      .IsRequired();
    }
  }
}