
using InventoryService.Test.Config;
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
            productService = new ProductService(Context);
        }
        [Fact]
        public async Task AddProductAsync()
        {
            //Arrange
            var product = new Product { Name = "Clock", Count = 10 };

            //Act
            var result = await productService.AddProductAsync(product);

            //Assert
            Assert.True(result);

        }
        [Fact]
        public async Task UpdateProductAsync()
        {
            //Arrange
            var product = new ProductDto { Name = "Mouse", Count = 3 };

            //Act
            var result = await productService.UpdateProductAsync(product);

            //Assert
            Assert.Equal(17, result.Count);

        }
        [Fact]
        public async Task CancelProductAsync()
        {
            //Arrange
            var product = new ProductDto { Name = "Mouse", Count = 20 };

            //Act
            var result = await productService.CancelProductAsync(product);

            //Assert
            Assert.True(result);

        }
    }
}
