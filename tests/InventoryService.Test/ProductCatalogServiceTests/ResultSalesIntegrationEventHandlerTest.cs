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
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.ProductCatalogServiceTests
{
    public class ResultSalesIntegrationEventHandlerTest : ProductCatalogMemoryDatabaseConfig
    {
        private ResultSalesIntegrationEventHandler resultSalesIntegrationEventHandler;

        public ResultSalesIntegrationEventHandlerTest()
        {
            var logger = new Mock<ILogger<ResultSalesIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var eventBus = new Mock<IEventBus>();

            resultSalesIntegrationEventHandler = new ResultSalesIntegrationEventHandler(logger.Object, productService, eventBus.Object);
        }


        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultSalesIntegrationEventHandler.Handle(null)));
        }

        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_Product_Id_Is_Zero_throw_ArgumentNullException()
        {
            // Arrange
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(0, false);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent)));
        }

        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_Product_Is_Invalid_throw_ArgumentNullException()
        {
            // Arrange
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(10, false);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent)));
        }

        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_IsSuccess_Is_False_Then_Delete_Product()
        {
            // Arrange
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(1, false);

            //Act 
            await resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent);
            var hasProduct = await Context.Products.AnyAsync(x => x.Id == resultSalesIntegrationEvent.ProductId);

            // Assert
            Assert.False(hasProduct);
        }

        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_IsSuccess_Is_True_Then_Update_ProductStatus()
        {
            // Arrange
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(3, true);

            //Act 
            await resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == resultSalesIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(ProductStatus.Completed, product.ProductStatus);
        }

    }
}
