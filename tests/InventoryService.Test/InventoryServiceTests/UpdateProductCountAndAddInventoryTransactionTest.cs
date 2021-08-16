using EventBus.Abstractions;
using InventoryService.IntegrationEvents.EventHandling;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Inventory
{
    public class UpdateProductCountAndAddInventoryTransactionTest : InventoryMemoryDatabaseConfig
    {
        private InventoryTransactionService inventoryTransactionService;
        private UpdateProductCountAndAddInventoryTransactionEventHandler updateProductCountAndAddInventoryTransaction;

        public UpdateProductCountAndAddInventoryTransactionTest()
        {
            var logger = new Mock<ILogger<UpdateProductCountAndAddInventoryTransactionEvent>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var loggerInventoryTransaction = new Mock<ILogger<InventoryTransactionService>>();
            inventoryTransactionService = new InventoryTransactionService(Context, loggerInventoryTransaction.Object);

            var eventBus = new Mock<IEventBus>();

            updateProductCountAndAddInventoryTransaction = new UpdateProductCountAndAddInventoryTransactionEventHandler(Context, logger.Object, productService, inventoryTransactionService, eventBus.Object);
        }


        [Fact]
        public async Task UpdateProductCountAndAddInventoryTransaction_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => updateProductCountAndAddInventoryTransaction.Handle(null)));
        }

    }
}
