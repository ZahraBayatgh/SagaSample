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
    public class SalesResultIntegrationEventHandlerTest : ProductCatalogMemoryDatabaseConfig
    {
        private SalesResultIntegrationEventHandler salesResultIntegrationEventHandler;
        private string correlationId;

        public SalesResultIntegrationEventHandlerTest()
        {
            correlationId = "123";

            var logger = new Mock<ILogger<SalesResultIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var eventBus = new Mock<IEventBus>();

            salesResultIntegrationEventHandler = new SalesResultIntegrationEventHandler(logger.Object, productService, eventBus.Object);
        }


        [Fact]
        public async Task SalesResultIntegrationEventHandler_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => salesResultIntegrationEventHandler.Handle(null)));
        }

        [Fact]
        public async Task SalesResultIntegrationEventHandler_When_Product_Id_Is_Zero_throw_ArgumentNullException()
        {
            // Arrange
            SalesResultIntegrationEvent salesResultIntegrationEvent = new SalesResultIntegrationEvent(0, false, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => salesResultIntegrationEventHandler.Handle(salesResultIntegrationEvent)));
        }

        [Fact]
        public async Task SalesResultIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Success_And_ProductStatus_In_Db_Is_Not_SalesIsOk_Update_Product_Status()
        {
            // Arrange
            SalesResultIntegrationEvent salesResultIntegrationEvent = new SalesResultIntegrationEvent(3, true, correlationId);

            //Act
            var salesResultIntegrationEventResponse = salesResultIntegrationEventHandler.Handle(salesResultIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == salesResultIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(ProductStatus.Completed, product.ProductStatus);
        }

        [Fact]
        public async Task SalesResultIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Failure_And_ProductStatus_In_Db_Is_InventoryIsOk_Delete_Product()
        {
            // Arrange
            SalesResultIntegrationEvent salesResultIntegrationEvent = new SalesResultIntegrationEvent(6, false, correlationId);

            //Act
            var salesResultIntegrationEventResponse = salesResultIntegrationEventHandler.Handle(salesResultIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == salesResultIntegrationEvent.ProductId);

            // Assert
            Assert.Null(product);
        }
    }
}
