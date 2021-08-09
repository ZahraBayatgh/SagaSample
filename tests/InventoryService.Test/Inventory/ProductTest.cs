using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InventoryService.Test
{
    public class ProductTest : InventoryMemoryDatabaseConfig
    {
        private ProductService productService;

        public ProductTest()
        {
            var logger = new Mock<ILogger<ProductService>>();

            productService = new ProductService(Context, logger.Object);
        }
        [Fact]
        public async Task AddProductAsync()
        {
            //Arrange
            var product = new Product
            {
                Id=5,
                Name = "Clock", 
                Count = 10
            };

            //Act
           var productId= await productService.AddProductAsync(product);

            //Assert
            Assert.Equal(5,productId);

        }
        [Fact]
        public async Task UpdateProductAsync()
        {
            //Arrange
            var productDto = new ProductDto
            {
                Name = "Mouse",
                Count = 5 
            };

            //Act
            var product = await productService.UpdateProductAsync(productDto);

            //Assert
            Assert.Equal(5, product.Count);

        }
    }
}
