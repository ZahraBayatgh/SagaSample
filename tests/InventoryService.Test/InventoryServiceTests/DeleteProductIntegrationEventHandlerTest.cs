using InventoryService.IntegrationEvents.EventHandling;
using InventoryService.Services;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Inventory
{
    public class DeleteProductIntegrationEventHandlerTest : InventoryMemoryDatabaseConfig
    {
        private InventoryTransactionService inventoryTransactionService;
        private DeleteProductIntegrationEventHandler deleteProductIntegrationEventHandler;

        public DeleteProductIntegrationEventHandlerTest()
        {
            var logger = new Mock<ILogger<DeleteProductIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var loggerInventoryTransaction = new Mock<ILogger<InventoryTransactionService>>();
            inventoryTransactionService = new InventoryTransactionService(Context, loggerInventoryTransaction.Object);
            deleteProductIntegrationEventHandler = new DeleteProductIntegrationEventHandler(Context, logger.Object, productService, inventoryTransactionService);
        }


        [Fact]
        public async Task DeleteProductIntegrationEvent_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => deleteProductIntegrationEventHandler.Handle(null)));
        }


    }
}
