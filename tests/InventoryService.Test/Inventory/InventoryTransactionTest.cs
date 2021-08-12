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
        private InventoryTransactionService inventoryTransactionService;

        public InventoryTransactionTest()
        {
            var logger = new Mock<ILogger<InventoryTransactionService>>();
            inventoryTransactionService = new InventoryTransactionService(Context, logger.Object);
        }

        #region GetLatestInventoryTransactionByProductId

        [Fact]
        public async Task GetLatestInventoryTransactionByProductId_When_ProducId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var product = await inventoryTransactionService.GetLatestInventoryTransactionByProductIdAsync(id);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task GetLatestInventoryTransactionByProductId_When_ProducId_Is_Valid_Return_InventoryTransactionCurrentCount()
        {
            //Arrange
            var id = 1;

            //Act
            var inventoryTransactionCurrentCount = await inventoryTransactionService.GetLatestInventoryTransactionByProductIdAsync(id);

            //Assert
            Assert.Equal(15, inventoryTransactionCurrentCount.Value);
        }

        #endregion

        #region CreateInventoryTransaction

        [Fact]
        public async Task CreateInventoryTransactionn_When_InventoryTransactionDto_Is_null_Return_Failure()
        {
            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(null);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateInventoryTransaction_When_ProductId_Is_Zero_Return_Failure()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 0,
                ChangeCount = 2,
                CurrentCount = 18
            };

            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateInventoryTransaction_When_ChangeCount_Is_Zero_Return_Failure()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 1,
                ChangeCount = 0,
                CurrentCount = 18
            };

            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateInventoryTransaction_When_CurrentCountt_Is_Zero_Return_Failure()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 1,
                ChangeCount = 2,
                CurrentCount = 0
            };

            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }

        [Fact]
        public async Task CreateInventoryTransaction_When_InventoryTransactionDto_Is_Valid_Return_InventoryTransactionDto()
        {
            //Arrange
            var inventoryTransactionDto = new InventoryTransactionDto
            {
                ProductId = 1,
                ChangeCount = 2,
                CurrentCount = 18
            };

            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionDto);

            //Assert
            Assert.Equal(18, inventoryTransaction.Value.CurrentCount);
        }

        #endregion

        #region DeleteInventoryTransaction

        [Fact]
        public async Task DeleteInventoryTransaction_When_InventoryTransaction_Is_Zero_Return_Failure()
        {
            //Arrange
            int inventoryTransactionId = 0;

            //Act
            var result = await inventoryTransactionService.DeleteInventoryTransactionAsync(inventoryTransactionId);

            //Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task DeleteInventoryTransaction_When_InventoryTransaction_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            int inventoryTransactionId = 30;

            //Act
            var createInventoryTransactionResponseDto = await inventoryTransactionService.DeleteInventoryTransactionAsync(inventoryTransactionId);

            //Assert
            Assert.True(createInventoryTransactionResponseDto.IsFailure);
        }


        [Fact]
        public async Task DeleteInventoryTransaction_When_InventoryTransaction_Is_Valid_Return_Success()
        {
            //Arrange
            int inventoryTransactionId = 1;

            //Act
            var UpdateInventoryTransactionCount = await inventoryTransactionService.DeleteInventoryTransactionAsync(inventoryTransactionId);

            //Assert
            Assert.True(UpdateInventoryTransactionCount.IsSuccess);
        }
        #endregion
    }
}
