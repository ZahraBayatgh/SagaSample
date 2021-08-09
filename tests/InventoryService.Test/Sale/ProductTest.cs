
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using SaleService.Dtos;
using SaleService.Models;
using SaleService.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Sale
{
    public class ProductTest : SaleMemoryDatabaseConfig
    {
        private ProductService productService;

        public ProductTest()
        {
            var logger = new Mock<ILogger<ProductService>>();
            productService = new ProductService(Context,logger.Object);
        }
        [Fact]
        public async Task AddProductAsync()
        {
            //Arrange
            var product = new Product 
            {
                Id=8,
                Name = "Clock",
                Count = 10 };

            //Act
            var productId = await productService.AddProductAsync(product);

            //Assert
            Assert.Equal(8,productId);

        }
        [Fact]
        public async Task UpdateProductAsync()
        {
            //Arrange
            var productDto = new ProductDto
            {
                Name = "Mouse",
                Count = 3 
            };

            //Act
            var product = await productService.UpdateProductAsync(productDto);

            //Assert
            Assert.Equal(17, product.Count);

        }
        [Fact]
        public async Task CancelProductAsync()
        {
            //Arrange
            var productDto = new ProductDto { Name = "Mouse", Count = 20 };

            //Act
            var result = await productService.CancelProductAsync(productDto);

            //Assert
            Assert.True(result);

        }
    }
}
