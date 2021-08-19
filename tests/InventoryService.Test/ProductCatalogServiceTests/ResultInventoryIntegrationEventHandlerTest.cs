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
    public class ResultInventoryIntegrationEventHandlerTest : ProductCatalogMemoryDatabaseConfig
    {
        private ResultInventoryIntegrationEventHandler resultInventoryIntegrationEventHandler;
        private string correlationId;

        public ResultInventoryIntegrationEventHandlerTest()
        {
            correlationId = "123";

            var logger = new Mock<ILogger<ResultInventoryIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var eventBus = new Mock<IEventBus>();

            resultInventoryIntegrationEventHandler = new ResultInventoryIntegrationEventHandler(logger.Object, productService, eventBus.Object);
        }


        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultInventoryIntegrationEventHandler.Handle(null)));
        }

        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_Product_Id_Is_Zero_throw_ArgumentNullException()
        {
            // Arrange
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(0, false,correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent)));
        }

        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Success_And_ProductStatus_In_Db_Is_Not_InventoryIsOk_Update_Product_Status()
        {
            // Arrange
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(4, true,correlationId);

            //Act
            var resultInventoryIntegrationEventResponse = resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == resultInventoryIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(ProductStatus.Completed, product.ProductStatus);
        }

        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Failure_And_ProductStatus_In_Db_Is_InventoryIsOk_Delete_Product()
        {
            // Arrange
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(5, false,correlationId);

            //Act 
            await resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == resultInventoryIntegrationEvent.ProductId);

            // Assert
            Assert.Null(product);
        }

    }
}
