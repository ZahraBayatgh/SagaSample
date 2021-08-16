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

        public ResultInventoryIntegrationEventHandlerTest()
        {
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
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(0, false);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent)));
        }

        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_Product_Is_Invalid_throw_ArgumentNullException()
        {
            // Arrange
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(10, false);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent)));
        }

        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_IsSuccess_Is_False_Then_Delete_Product()
        {
            // Arrange
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(1, false);

            //Act 
            await resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent);
            var hasProduct = await Context.Products.AnyAsync(x => x.Id == resultInventoryIntegrationEvent.ProductId);

            // Assert
            Assert.False(hasProduct);
        }

        [Fact]
        public async Task ResultInventoryIntegrationEventHandler_When_IsSuccess_Is_True_Then_Update_ProductStatus()
        {
            // Arrange
            ResultInventoryIntegrationEvent resultInventoryIntegrationEvent = new ResultInventoryIntegrationEvent(2, true);

            //Act 
            await resultInventoryIntegrationEventHandler.Handle(resultInventoryIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == resultInventoryIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(ProductStatus.Completed, product.ProductStatus);
        }

    }
}
