using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Test.Config;
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
            inventoryService = new InventoryService.Services.InventoryService(Context);
        }
        [Fact]
        public async Task AddProductAsync()
        {
            //Arrange
            var inventory = new InventoryService.Models.Inventory {  ProductId=1 ,Count=2,Type= InventoryService.Models.Type.Out };

            //Act
            var result = await inventoryService.AddInventoryAsync(inventory);

            //Assert
            Assert.True(result);

        }
    }
}
