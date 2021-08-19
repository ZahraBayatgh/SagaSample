using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SalesService.Dtos;
using SalesService.IntegrationEvents.Events;
using SalesService.Services;
using System;
using System.Threading.Tasks;

namespace SalesService.IntegrationEvents.EventHandling
{
    public class CreateProductIntegrationEventHandler : IIntegrationEventHandler<CreateProductIntegrationEvent>
    {
        private readonly ILogger<CreateProductIntegrationEventHandler> _logger;
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;

        public CreateProductIntegrationEventHandler(ILogger<CreateProductIntegrationEventHandler> logger,
            IProductService productService,
            IEventBus eventBus)
        {
            _logger = logger;
            _productService = productService;
            _eventBus = eventBus;
        }
        public async Task Handle(CreateProductIntegrationEvent @event)
        {
            try
            {
                // Check CreateProductIntegrationEvent
                CheckCreateProductIntegrationEventInstance(@event);

                // Create product
                var createProductRequestDto = new CreateProductRequestDto
                {
                    Name = @event.ProductName,
                    Count = @event.InitialOnHand
                };
                var createProductResponce = await _productService.CreateProductAsync(createProductRequestDto);

                // Publish ResultSalesIntegrationEvent
                bool createProductStatus = createProductResponce.IsSuccess ? true : false;
                await PublishResult(@event, createProductStatus);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogInformation($"CreateProductIntegrationEvent is null. Exception detail:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Product {@event.ProductName} wan not created. Exception detail:{ex.Message}");

                // Publish ResultSalesIntegrationEvent
                await PublishResult(@event, false);

                throw;
            }
        }

        private async Task PublishResult(CreateProductIntegrationEvent @event, bool createProductStatus)
        {
            // Publish ResultSalesIntegrationEvent
            ResultSalesIntegrationEvent resultSalesIntegrationEvent = new ResultSalesIntegrationEvent(@event.ProductId, createProductStatus, @event.CorrelationId);
            await _eventBus.PublishAsync(resultSalesIntegrationEvent);
        }

        private static void CheckCreateProductIntegrationEventInstance(CreateProductIntegrationEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException("CreateProductIntegrationEvent is null.");

            if (@event.ProductId <= 0)
                throw new ArgumentNullException("CreateProductIntegrationEvent ProductId is invalid.");

            if (string.IsNullOrEmpty(@event.ProductName))
                throw new ArgumentNullException("ResultSalesIntegrationEvent ProductName is null.");
        }
    }
}
