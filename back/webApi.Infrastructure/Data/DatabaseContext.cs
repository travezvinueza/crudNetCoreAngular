
using Microsoft.EntityFrameworkCore;
using webApi.Domain.Models;

namespace webApi.Infrastructure.Data
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
                new Product { Id = 5, Name = "Laptop", Price = 155.75M },
                new Product { Id = 6, Name = "Mouse", Price = 10.50M },
                new Product { Id = 7, Name = "Impresora", Price = 99.99M },
                new Product { Id = 8, Name = "Webcam", Price = 25.55M },
                new Product { Id = 9, Name = "Router", Price = 45.77M },
                new Product { Id = 10, Name = "Cargador", Price = 15.88M },
                new Product { Id = 11, Name = "Pendrive", Price = 8.99M },
                new Product { Id = 12, Name = "Disco Duro", Price = 99.33M },
                new Product { Id = 13, Name = "Tarjeta de Video", Price = 250.75M },
                new Product { Id = 14, Name = "Tarjeta Madre", Price = 150.25M },
                new Product { Id = 15, Name = "Fuente de Poder", Price = 75.95M },
                new Product { Id = 16, Name = "Memoria RAM", Price = 60.99M }
            );
        }
    }
}