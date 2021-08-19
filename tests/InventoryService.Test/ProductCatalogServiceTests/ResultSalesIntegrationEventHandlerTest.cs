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
    public class ResultSalesIntegrationEventHandlerTest : ProductCatalogMemoryDatabaseConfig
    {
        private ResultSalesIntegrationEventHandler resultSalesIntegrationEventHandler;
        private string correlationId;

        public ResultSalesIntegrationEventHandlerTest()
        {
            correlationId = "123";

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
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(0, false,correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent)));
        }

        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Success_And_ProductStatus_In_Db_Is_Not_SalesIsOk_Update_Product_Status()
        {
            // Arrange
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(3, true,correlationId);

            //Act
           var resultSalesIntegrationEventResponse=  resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == resultSalesIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(ProductStatus.Completed, product.ProductStatus);
        }

        [Fact]
        public async Task ResultSalesIntegrationEventHandler_When_Product_Is_In_Db_And_Event_Is_Failure_And_ProductStatus_In_Db_Is_InventoryIsOk_Delete_Product()
        {
            // Arrange
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(6, false,correlationId);

            //Act
            var resultSalesIntegrationEventResponse = resultSalesIntegrationEventHandler.Handle(resultSalesIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == resultSalesIntegrationEvent.ProductId);

            // Assert
            Assert.Null(product);
        }
    }
}
