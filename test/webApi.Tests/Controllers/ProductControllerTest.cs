using Moq;
using webApi.Api.Controllers;
using webApi.Application.Dtos;
using webApi.Application.Interfaces;
using webApi.Domain.Models;

namespace webApi.Tests.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> productServiceMock;
        private readonly Mock<Microsoft.Extensions.Logging.ILogger<ProductController>> loggerMock;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            productServiceMock = new Mock<IProductService>();
            loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<ProductController>>();
            _controller = new ProductController(productServiceMock.Object, loggerMock.Object);
        }

        [Fact]
        public void GetProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1", Price = 10.0M },
                new Product { Id = 2, Name = "Product2", Price = 20.0M }
            };
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
            productServiceMock.Setup(service => service.GetAllAsync()).Returns(Task.FromResult<IEnumerable<ProductDto>>(productDtos));

            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.NotNull(result);
        }
    }
}