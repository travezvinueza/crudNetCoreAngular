using Microsoft.EntityFrameworkCore;
using webApi.Model;

namespace webApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product1", Price = 10.0M },
                new Product { Id = 2, Name = "Product2", Price = 20.0M },
                new Product { Id = 3, Name = "Product3", Price = 30.0M },
                new Product { Id = 4, Name = "Product4", Price = 40.0M },
                new Product { Id = 5, Name = "Product5", Price = 50.0M },
                new Product { Id = 6, Name = "Product6", Price = 60.0M },
                new Product { Id = 7, Name = "Product7", Price = 70.0M },
                new Product { Id = 8, Name = "Product8", Price = 80.0M },
                new Product { Id = 9, Name = "Product9", Price = 90.0M },
                new Product { Id = 10, Name = "Product10", Price = 100.0M }
            );
        }
    }
}