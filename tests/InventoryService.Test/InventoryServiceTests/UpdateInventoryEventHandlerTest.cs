using EventBus.Abstractions;
using InventoryService.IntegrationEvents.EventHandling;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Inventory
{
    public class UpdateInventoryEventHandlerTest : InventoryMemoryDatabaseConfig
    {
        private InventoryTransactionService inventoryTransactionService;
        private UpdateInventoryEventHandler updateProductCountAndAddInventoryTransaction;
        private string correlationId;

        public UpdateInventoryEventHandlerTest()
        {
            correlationId = "123";

            var logger = new Mock<ILogger<UpdateInventoryEvent>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var loggerInventoryTransaction = new Mock<ILogger<InventoryTransactionService>>();
            inventoryTransactionService = new InventoryTransactionService(Context, loggerInventoryTransaction.Object);

            var eventBus = new Mock<IEventBus>();

            updateProductCountAndAddInventoryTransaction = new UpdateInventoryEventHandler(Context, logger.Object, productService, inventoryTransactionService, eventBus.Object);
        }


        [Fact]
        public async Task UpdateProductCountAndAddInventoryTransaction_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => updateProductCountAndAddInventoryTransaction.Handle(null)));
        }

        [Fact]
        public async Task UpdateProductCountAndAddInventoryTransaction_When_Product_Is_Not_In_Db_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => updateProductCountAndAddInventoryTransaction.Handle(null)));
        }

        [Fact]
        public async Task UpdateProductCountAndAddInventoryTransaction_When_EveryThing_Is_OK_Update_Product()
        {
            // Arrange
            UpdateInventoryEvent updateInventoryEvent = new UpdateInventoryEvent("Mouse", 10,1,1,correlationId);

            //Act 
            await updateProductCountAndAddInventoryTransaction.Handle(updateInventoryEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Name == updateInventoryEvent.ProductName);

            // Assert
            Assert.Equal(1, product.Id);
        }

    }
}
