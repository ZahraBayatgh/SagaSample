using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SaleService.IntegrationEvents.EventHandling;
using SaleService.IntegrationEvents.Events;
using SaleService.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class DeleteInventoryIntegrationEventHandlerTest : SalesMemoryDatabaseConfig
    {
        private DeleteSalesIntegrationEventHandler deleteSalesIntegrationEventHandler;

        public DeleteInventoryIntegrationEventHandlerTest()
        {
            var logger = new Mock<ILogger<DeleteSalesIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);


            deleteSalesIntegrationEventHandler = new DeleteSalesIntegrationEventHandler(logger.Object, productService);
        }


        [Fact]
        public async Task DeleteSalesIntegrationEvent_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => deleteSalesIntegrationEventHandler.Handle(null)));
        }

        [Fact]
        public async Task DeleteSalesIntegrationEvent_When_Product_Name_Is_Null_throw_ArgumentNullException()
        {
            // Arrange
            DeleteSalesIntegrationEvent deleteSalesIntegrationEvent = new DeleteSalesIntegrationEvent(null);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => deleteSalesIntegrationEventHandler.Handle(deleteSalesIntegrationEvent)));
        }

        [Fact]
        public async Task DeleteSalesIntegrationEvent_When_Product_Name_Is_Empty_throw_ArgumentNullException()
        {
            // Arrange
            DeleteSalesIntegrationEvent deleteSalesIntegrationEvent = new DeleteSalesIntegrationEvent("");

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => deleteSalesIntegrationEventHandler.Handle(deleteSalesIntegrationEvent)));
        }

        [Fact]
        public async Task DeleteSalesIntegrationEvent_When_Everything_Is_OK_Create_Product()
        {
            // Arrange
            DeleteSalesIntegrationEvent deleteSalesIntegrationEvent = new DeleteSalesIntegrationEvent("Mouse");

            //Act 
            await deleteSalesIntegrationEventHandler.Handle(deleteSalesIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Name == deleteSalesIntegrationEvent.ProductName);

            // Assert
            Assert.Null(product);
        }
    }
}
