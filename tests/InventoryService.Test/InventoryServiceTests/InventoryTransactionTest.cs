using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.InventoryServiceTests
{
    public class InventoryTransactionTest : InventoryMemoryDatabaseConfig
    {
        private InventoryTransactionService inventoryTransactionService;

        public InventoryTransactionTest()
        {
            var logger = new Mock<ILogger<InventoryTransactionService>>();
            inventoryTransactionService = new InventoryTransactionService(Context, logger.Object);
        }

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
            var inventoryTransactionRequestDto = new InventoryTransactionRequestDto(0, 18,  InventoryType.Out);

            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionRequestDto);

            //Assert
            Assert.True(inventoryTransaction.IsFailure);
        }
        [Fact]
        public async Task CreateInventoryTransaction_When_InventoryTransactionDto_Is_Valid_Return_InventoryTransactionDto()
        {
            //Arrange
            var inventoryTransactionRequestDto = new InventoryTransactionRequestDto(1, 18,  InventoryType.Out);
           
            //Act
            var inventoryTransaction = await inventoryTransactionService.CreateInventoryTransactionAsync(inventoryTransactionRequestDto);

            //Assert
            Assert.Equal(18, inventoryTransaction.Value.Count);
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
