
using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SalesService.IntegrationEvents.EventHandling;
using SalesService.IntegrationEvents.Events;
using SalesService.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class CreateProductIntegrationEventHandlerTest : SalesMemoryDatabaseConfig
    {
        private CreateProductIntegrationEventHandler createProductIntegrationEventHandler;
        private string correlationId;

        public CreateProductIntegrationEventHandlerTest()
        {
            correlationId = "123";

            var logger = new Mock<ILogger<CreateProductIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var eventBus = new Mock<IEventBus>();

            createProductIntegrationEventHandler = new CreateProductIntegrationEventHandler(logger.Object, productService, eventBus.Object);
        }


        [Fact]
        public async Task CreateProductIntegrationEvent_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => createProductIntegrationEventHandler.Handle(null)));
        }

        [Fact]
        public async Task CreateProductIntegrationEvent_When_Product_Id_Is_Zero_throw_ArgumentNullException()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(0, "Flash", 10, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => createProductIntegrationEventHandler.Handle(createProductIntegrationEvent)));
        }
        [Fact]
        public async Task CreateProductIntegrationEvent_When_Product_Name_Is_Null_throw_ArgumentNullException()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(0, null, 10, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => createProductIntegrationEventHandler.Handle(createProductIntegrationEvent)));
        }

        [Fact]
        public async Task CreateProductIntegrationEvent_When_Product_Name_Is_Empty_throw_ArgumentNullException()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(0, "", 10, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>((() => createProductIntegrationEventHandler.Handle(createProductIntegrationEvent)));
        }

        [Fact]
        public async Task CreateProductIntegrationEvent_When_Everything_Is_OK_Create_Product()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(3, "Clock", 10, correlationId);

            //Act 
            await createProductIntegrationEventHandler.Handle(createProductIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == createProductIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(3, product.Id);
        }
    }
}
