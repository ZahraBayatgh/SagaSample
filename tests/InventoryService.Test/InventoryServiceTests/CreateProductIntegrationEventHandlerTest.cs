using EventBus.Abstractions;
using InventoryService.IntegrationEvents.EventHandling;
using InventoryService.IntegrationEvents.Events;
using InventoryService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.InventoryServiceTests
{
    public class CreateProductIntegrationEventHandlerTest : InventoryMemoryDatabaseConfig
    {
        private CreateProductIntegrationEventHandler createProductIntegrationEventHandler;
        private string correlationId;

        public CreateProductIntegrationEventHandlerTest()
        {
            correlationId = "123";
            var logger = new Mock<ILogger<CreateProductIntegrationEventHandler>>();

            var loggerProduct = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, loggerProduct.Object);

            var loggerInventoryTransaction = new Mock<ILogger<InventoryTransactionService>>();
           var inventoryTransactionService = new InventoryTransactionService(Context, loggerInventoryTransaction.Object);

            var eventBus = new Mock<IEventBus>();

            createProductIntegrationEventHandler = new CreateProductIntegrationEventHandler(logger.Object,Context, productService, inventoryTransactionService, eventBus.Object);
        }


        [Fact]
        public async Task CreateProductIntegrationEvent_When_Input_Is_Null_throw_ArgumentNullException()
        {
            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => createProductIntegrationEventHandler.Handle(null));
        }

        [Fact]
        public async Task CreateProductIntegrationEvent_When_Product_Id_Is_Zero_throw_ArgumentNullException()
        {
            // Arrange
           CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(0, "Flash", 10, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => createProductIntegrationEventHandler.Handle(createProductIntegrationEvent));
        }
        [Fact]
        public async Task CreateProductIntegrationEvent_When_Product_Name_Is_Null_throw_ArgumentNullException()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(1, null, 10, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => createProductIntegrationEventHandler.Handle(createProductIntegrationEvent));
        }

        [Fact]
        public async Task CreateProductIntegrationEvent_When_Product_Name_Is_Empty_throw_ArgumentNullException()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(1, "", 10, correlationId);

            //Act - Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => createProductIntegrationEventHandler.Handle(createProductIntegrationEvent));
        }

        [Fact]
        public async Task CreateProductIntegrationEvent_When_Everything_Is_OK_Create_Product()
        {
            // Arrange
            CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(3, "Hub", 10, correlationId);

            //Act 
            await createProductIntegrationEventHandler.Handle(createProductIntegrationEvent);
            var product = await Context.Products.FirstOrDefaultAsync(x => x.Id == createProductIntegrationEvent.ProductId);

            // Assert
            Assert.Equal(3, product.Id);
        }
    }
}
