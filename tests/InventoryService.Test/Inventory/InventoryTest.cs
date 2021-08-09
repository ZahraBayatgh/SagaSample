using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Inventory
{
    public class InventoryTest : InventoryMemoryDatabaseConfig
    {
        private InventoryService.Services.InventoryService inventoryService;

        public InventoryTest()
        {
            var logger = new Mock<ILogger<InventoryService.Services.InventoryService>>();
            inventoryService = new InventoryService.Services.InventoryService(Context, logger.Object);
        }
        [Fact]
        public async Task AddProductAsync()
        {
            //Arrange
            var inventory = new InventoryService.Models.Inventory
            { 
                Id=6,
                ProductId=1 ,
                Count=2,
                Type= InventoryType.Out
            };

            //Act
            var inventoryId = await inventoryService.AddInventoryAsync(inventory);

            //Assert
            Assert.Equal(6,inventoryId);

        }
    }
}
