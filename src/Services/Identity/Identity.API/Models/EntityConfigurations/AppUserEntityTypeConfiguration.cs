using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Identity.API.Models;

namespace Identity.API.Models.EntityConfigurations
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
  {
    public void Configure(EntityTypeBuilder<Identity.API.Models.AppUser> b)
    {
        // Maps to the AspNetUsers table
        b.ToTable("AspNetUsers");
        b.HasIndex(a => a.NormalizedEmail).IsUnique();
        b.HasIndex(a => a.NormalizedEmail).IsUnique();
       
        b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        // A concurrency token for use with the optimistic concurrency checking
        b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

        // Limit the size of columns to use efficient database types
        b.Property(u => u.UserName).HasMaxLength(256);
        b.Property(u => u.NormalizedUserName).HasMaxLength(256);
        b.Property(u => u.Email).HasMaxLength(256);
        b.Property(u => u.NormalizedEmail).HasMaxLength(256);
    }
  }
}