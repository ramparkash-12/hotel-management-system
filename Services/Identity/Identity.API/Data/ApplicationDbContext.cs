using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Identity.API.Models;
using Identity.API.Models.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.Data
{
  public class ApplicationDbContext : IdentityDbContext<AppUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.ApplyConfiguration(new AppUserEntityTypeConfiguration());
      //builder.ApplyConfiguration(new RoleEntityTypeConfiguration()); 

      //base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);
    }

    public class ApplicationDbContextContextDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=tcp:localhost,5434;Initial Catalog=IdentityAPI;User Id=sa;Password=iamback.786A@;Integrated Security=true");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
  }
}