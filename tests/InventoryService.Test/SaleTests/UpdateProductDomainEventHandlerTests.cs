
using EventBus.Abstractions;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using SaleService.DomainEvents.EventHandling;
using SaleService.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class UpdateProductDomainEventHandlerTests : SaleMemoryDatabaseConfig
    {
        private UpdateProductDomainEventHandler updateProductDomainEventHandler;

        public UpdateProductDomainEventHandlerTests()
        {
            var logger = new Mock<ILogger<UpdateProductDomainEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var loggerOrder = new Mock<ILogger<OrderService>>();
            var orderService = new OrderService(Context, loggerOrder.Object, productService);

            var eventBus = new Mock<IEventBus>();

            updateProductDomainEventHandler = new UpdateProductDomainEventHandler(productService, orderService, eventBus.Object, logger.Object);
        }


        [Fact]
        public async Task UpdateProductDomainEventHandler_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => updateProductDomainEventHandler.Handle(null, new System.Threading.CancellationToken())));
        }
    }
}
