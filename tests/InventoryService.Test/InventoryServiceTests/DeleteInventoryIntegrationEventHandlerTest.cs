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

namespace SagaPattern.UnitTests.InventoryServiceTests
{
    public class DeleteInventoryIntegrationEventHandlerTest : InventoryMemoryDatabaseConfig
    {
        private DeleteInventoryIntegrationEventHandler deleteInventoryIntegrationEventHandler;

        public DeleteInventoryIntegrationEventHandlerTest()
        {
            var logger = new Mock<ILogger<DeleteInventoryIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);


            deleteInventoryIntegrationEventHandler = new DeleteInventoryIntegrationEventHandler(logger.Object, productService);
        }


        [Fact]
        public async Task DeleteInventoryIntegrationEvent_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => deleteInventoryIntegrationEventHandler.Handle(null));
        }

        [Fact]
        public async Task DeleteInventoryIntegrationEvent_When_Product_Name_Is_Null_throw_ArgumentNullException()
        {
            // Arrange
            DeleteInventoryIntegrationEvent deleteSalesIntegrationEvent = new DeleteInventoryIntegrationEvent(null);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => deleteInventoryIntegrationEventHandler.Handle(deleteSalesIntegrationEvent));
        }

        [Fact]
        public async Task DeleteInventoryIntegrationEvent_When_Product_Name_Is_Empty_throw_ArgumentNullException()
        {
            // Arrange
            DeleteInventoryIntegrationEvent deleteSalesIntegrationEvent = new DeleteInventoryIntegrationEvent("");

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => deleteInventoryIntegrationEventHandler.Handle(deleteSalesIntegrationEvent));
        }

        [Fact]
        public async Task DeleteInventoryIntegrationEvent_When_Everything_Is_OK_Create_Product()
        {
            // Arrange
            DeleteInventoryIntegrationEvent deleteSalesIntegrationEvent = new DeleteInventoryIntegrationEvent("Mouse");

            //Act 
            await deleteInventoryIntegrationEventHandler.Handle(deleteSalesIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Name == deleteSalesIntegrationEvent.ProductName);

            // Assert
            Assert.Null(product);
        }
    }
}
