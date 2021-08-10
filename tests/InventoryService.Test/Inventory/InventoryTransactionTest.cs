using InventoryService.Dtos;
using InventoryService.Services;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Inventory
{
    public class InventoryTransactionTest : InventoryMemoryDatabaseConfig
    {
        private InventoryTransactionService inventoryService;

        public InventoryTransactionTest()
        {
            var logger = new Mock<ILogger<InventoryTransactionService>>();
            inventoryService = new InventoryTransactionService(Context, logger.Object);
        }

        #region GetLatestInventoryTransactionByProductId

        [Fact]
        public async Task GetLatestInventoryTransactionByProductId_When_ProducId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var product = await inventoryService.GetLatestInventoryTransactionByProductIdAsync(id);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task GetLatestInventoryTransactionByProductId_When_ProducId_Is_Valid_Return_InventoryTransactionCurrentCount()
        {
            //Arrange
            var id = 1;

            //Act
            var inventoryTransactionCurrentCount = await inventoryService.GetLatestInventoryTransactionByProductIdAsync(id);

            //Assert
            Assert.Equal(15, inventoryTransactionCurrentCount.Value);
        }

        #endregion

        #region InventoryTransaction

        [Fact]
        public async Task InventoryTransaction_When_InventoryTransactionDto_Is_null_Return_Failure()
        {
            //Act
            var inventoryTransaction = await inventoryService.CreateInventoryTransactionAsync(null);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task InventoryTransaction_When_ProductId_Is_Zero_Return_Failure()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 0,
                ChangeCount = 2,
                CurrentCount = 18
            };

            //Act
            var inventoryTransaction = await inventoryService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task InventoryTransaction_When_ChangeCount_Is_Zero_Return_Failure()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 1,
                ChangeCount = 0,
                CurrentCount = 18
            };

            //Act
            var inventoryTransaction = await inventoryService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task InventoryTransaction_When_CurrentCountt_Is_Zero_Return_Failure()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 1,
                ChangeCount = 2,
                CurrentCount = 0
            };

            //Act
            var inventoryTransaction = await inventoryService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task InventoryTransaction_When_InventoryTransactionDto_Is_Valid_Return_InventoryTransactionDto()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 1,
                ChangeCount = 2,
                CurrentCount = 18
            };

            //Act
            var inventoryTransaction = await inventoryService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.Equal(18, inventoryTransaction.Value.CurrentCount);
        }

        #endregion
    }
}
