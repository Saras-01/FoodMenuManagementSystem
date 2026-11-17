using FoodMenuAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodMenuAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FoodItem> FoodItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FoodItem>()
                .Property(f => f.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FoodItem>()
                .HasIndex(f => f.Name)
                .IsUnique();
        }
    }
}