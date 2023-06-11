using Microsoft.EntityFrameworkCore;
using orion.Models;

namespace orion.Data
{
    public class OrionDbContext : DbContext
    {
        // DbSet properties for your entities
        public DbSet<User> Users { get; set; }

        // Constructor that accepts DbContextOptions
        public OrionDbContext(DbContextOptions<OrionDbContext> options) : base(options)
        {
        }

        // Override OnModelCreating if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity mappings and relationships if needed
        }
    }
}
