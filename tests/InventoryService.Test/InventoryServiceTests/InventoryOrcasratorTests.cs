using EventBus.Abstractions;
using InventoryService.Dtos;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.InventoryServiceTests
{
    public class InventoryOrcasratorTests : InventoryMemoryDatabaseConfig
    {
        private InventoryOrcasrator inventoryOrcasrator;

        public InventoryOrcasratorTests()
        {
            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var loggerInventoryTransaction = new Mock<ILogger<InventoryTransactionService>>();
            var inventoryTransactionService = new InventoryTransactionService(Context, loggerInventoryTransaction.Object);
            var eventBus = new Mock<IEventBus>();


            var logger = new Mock<ILogger<InventoryOrcasrator>>();

            inventoryOrcasrator = new InventoryOrcasrator(productService, inventoryTransactionService, logger.Object, eventBus.Object, Context);
        }

        #region CreateProductAndInventoryTransaction

        [Fact]
        public async Task CreateProductAndInventoryTransaction_When_Product_Is_null_Return_Failure()
        {
            //Act
            var createProductAndInventoryTransaction = await inventoryOrcasrator.CreateProductAndInventoryTransactionAsync(null);

            //Assert
            Assert.True(createProductAndInventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateProductAndInventoryTransaction_When_Product_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var productDto = new ProductDto
            {
                Count = 10
            };

            //Act
            var createProductAndInventoryTransaction = await inventoryOrcasrator.CreateProductAndInventoryTransactionAsync(productDto);

            //Assert
            Assert.True(createProductAndInventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateProductAndInventoryTransaction_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var productDto = new ProductDto
            {
                ProductName = "",
                Count = 10
            };

            //Act
            var createProductAndInventoryTransaction = await inventoryOrcasrator.CreateProductAndInventoryTransactionAsync(productDto);

            //Assert
            Assert.True(createProductAndInventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateProductAndInventoryTransaction_When_Product_Count_Is_Zero_Return_Failure()
        {
            //Arrange
            var productDto = new ProductDto
            {
                ProductName = "Pen",
                Count = 0
            };

            //Act
            var createProductAndInventoryTransaction = await inventoryOrcasrator.CreateProductAndInventoryTransactionAsync(productDto);

            //Assert
            Assert.True(createProductAndInventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateProductAndInventoryTransaction_When_Product_Is_Valid_Return_ProductId()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                ProductName = "Pen",
                Count = 10
            };

            //Act
            var createProductAndInventoryTransactionAsync = await this.inventoryOrcasrator.CreateProductAndInventoryTransactionAsync(createProductDto);

            //Assert
            Assert.Equal(3, createProductAndInventoryTransactionAsync.Value);
        }

        #endregion
    }
}
