using Identity.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data
{
    public class IdentityDbContext: DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        } 
    }
}