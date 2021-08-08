using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Test.Config;
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
            productService = new ProductService(Context);
        }
        [Fact]
        public async Task AddProductAsync()
        {
            //Arrange
            var product = new Product { Name = "Clock", Count = 10 };

            //Act
           var result= await productService.AddProductAsync(product);

            //Assert
            Assert.True(result);

        }
        [Fact]
        public async Task UpdateProductAsync()
        {
            //Arrange
            var product = new ProductDto { Name = "Mouse", Count = 5 };

            //Act
            var result = await productService.UpdateProductAsync(product);

            //Assert
            Assert.Equal(5, result.Count);

        }
    }
}
