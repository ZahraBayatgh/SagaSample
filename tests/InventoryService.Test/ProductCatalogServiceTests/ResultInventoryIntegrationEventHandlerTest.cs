using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalogService.IntegrationEvents.EventHandling;
using ProductCatalogService.IntegrationEvents.Events;
using ProductCatalogService.Models;
using ProductCatalogService.Services;
using SagaPattern.UnitTests.Config;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.ProductCatalogServiceTests
{
    public class InventoryResultIntegrationEventHandlerTest : ProductCatalogMemoryDatabaseConfig
    {
        private InventoryResultIntegrationEventHandler inventoryResultIntegrationEventHandler;
        private string correlationId;

        public InventoryResultIntegrationEventHandlerTest()
        {
            correlationId = "123";

            var logger = new Mock<ILogger<InventoryResultIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var eventBus = new Mock<IEventBus>();

            inventoryResultIntegrationEventHandler = new InventoryResultIntegrationEventHandler(logger.Object, productService, eventBus.Object);
        }


        [Fact]
        public async Task InventoryResultIntegrationEventHandler_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => inventoryResultIntegrationEventHandler.Handle(null)));
        }

        [Fact]
        public async Task InventoryResultIntegrationEventHandler_When_Product_Id_Is_Zero_throw_ArgumentNullException()
        {
            // Arrange
            InventoryResultIntegrationEvent inventoryResultIntegrationEvent = new InventoryResultIntegrationEvent(0, false, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => inventoryResultIntegrationEventHandler.Handle(inventoryResultIntegrationEvent)));
        }

        [Fact]
        public async Task InventoryResultIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Success_And_ProductStatus_In_Db_Is_Not_InventoryIsOk_Update_Product_Status()
        {
            // Arrange
            InventoryResultIntegrationEvent inventoryResultIntegrationEvent = new InventoryResultIntegrationEvent(4, true, correlationId);

            //Act
            var inventoryResultIntegrationEventResponse = inventoryResultIntegrationEventHandler.Handle(inventoryResultIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == inventoryResultIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(ProductStatus.Completed, product.ProductStatus);
        }

        [Fact]
        public async Task InventoryResultIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Failure_And_ProductStatus_In_Db_Is_InventoryIsOk_Delete_Product()
        {
            // Arrange
            InventoryResultIntegrationEvent inventoryResultIntegrationEvent = new InventoryResultIntegrationEvent(5, false, correlationId);

            //Act 
            await inventoryResultIntegrationEventHandler.Handle(inventoryResultIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == inventoryResultIntegrationEvent.ProductId);

            // Assert
            Assert.Null(product);
        }

    }
}
