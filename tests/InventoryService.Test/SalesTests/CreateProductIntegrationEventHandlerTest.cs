
using EventBus.Abstractions;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using SaleService.IntegrationEvents.EventHandling;
using SaleService.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class CreateProductIntegrationEventHandlerTest : SalesMemoryDatabaseConfig
    {
        private CreateProductIntegrationEventHandler createProductIntegrationEventHandler;

        public CreateProductIntegrationEventHandlerTest()
        {
            var logger = new Mock<ILogger<CreateProductIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var eventBus = new Mock<IEventBus>();

            createProductIntegrationEventHandler = new CreateProductIntegrationEventHandler(logger.Object, productService, eventBus.Object);
        }


        [Fact]
        public async Task UpdateProductCountAndAddInventoryTransaction_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => createProductIntegrationEventHandler.Handle(null)));
        }
    }
}
