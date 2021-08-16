using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SaleService.IntegrationEvents.EventHandling;
using SaleService.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class CancelChangeProductCountIntegrationEventHandlerTests : SalesMemoryDatabaseConfig
    {
        private CancelChangeProductCountIntegrationEventHandler cancelChangeProductCountIntegrationEventHandler;

        public CancelChangeProductCountIntegrationEventHandlerTests()
        {
            var logger = new Mock<ILogger<CancelChangeProductCountIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            cancelChangeProductCountIntegrationEventHandler = new CancelChangeProductCountIntegrationEventHandler(logger.Object, productService);
        }


        [Fact]
        public async Task UpdateProductCountAndAddInventoryTransaction_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => cancelChangeProductCountIntegrationEventHandler.Handle(null)));
        }
    }
}
