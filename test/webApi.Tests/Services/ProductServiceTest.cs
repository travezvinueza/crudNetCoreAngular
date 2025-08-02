using Microsoft.EntityFrameworkCore;
using webApi.Application.Mappings;
using webApi.Application.Services;

using webApi.Domain.Models;
using webApi.Infrastructure.Data;
using webApi.Infrastructure.Repositories;


namespace webApi.Tests.Services
{
    public class ProductServiceTest
    {
        private static DatabaseContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new DatabaseContext(options);
        }

        [Fact]
        public void GetProducts()
        {
            // Arrange
            var dbContext = GetDbContext();
            var productRepository = new ProductRepository(dbContext);
            var productMapper = new ProductMapper();
            var productService = new ProductService(productRepository, productMapper);
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.0M },
                new Product { Id = 2, Name = "Product2", Price = 20.0M }
            };
            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();

            // Act
            var result = productService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
        }
        
    }
}