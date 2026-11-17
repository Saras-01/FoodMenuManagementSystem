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

        public DbSet<FoodCategory> FoodCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FoodItem configuration
            modelBuilder.Entity<FoodItem>()
                .Property(f => f.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FoodItem>()
                .HasIndex(f => f.Name)
                .IsUnique();

            // FoodCategory configuration
            modelBuilder.Entity<FoodCategory>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}