using Microsoft.EntityFrameworkCore;
using orion.Models;

namespace orion.Data
{
    public class OrionDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }

        public OrionDbContext(DbContextOptions<OrionDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notes>()
          .HasOne(n => n.User)
          .WithMany(u => u.Notes)
          .HasForeignKey(n => n.UserId)
          .OnDelete(DeleteBehavior.Cascade);

        }

    }
}