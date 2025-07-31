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
                new Product { Id = 1, Name = "Teclado", Price = 12.99M },
                new Product { Id = 2, Name = "Parlantes", Price = 20.25M },
                new Product { Id = 3, Name = "Monitor", Price = 30.30M },
                new Product { Id = 4, Name = "USB", Price = 40.25M },
                new Product { Id = 5, Name = "Laptop", Price = 155.75M }
            );
        }
    }
}