using CSharpFunctionalExtensions;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using ProductCatalog.Data;
using ProductCatalog.Dtos;
using ProductCatalogService.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace ProductCatalogService.Services
{
    public class ProductOrchestratorService :  IProductOrchestratorService
    {
        private readonly ProductCatalogDbContext _context;
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<ProductOrchestratorService> _logger;

        public ProductOrchestratorService(ProductCatalogDbContext context,
            IProductService productService,
            IEventBus eventBus,
            ILogger<ProductOrchestratorService> logger)
        {
            _context = context;
            _productService = productService;
            _eventBus = eventBus;
            _logger = logger;
        }
        public async Task<Result<int>> CreateProductAndPublishEvent(CreateProductRequestDto createProductRequestDto, string correlationId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var createProductResponse = await _productService.CreateProductAsync(createProductRequestDto);

                if (createProductResponse.IsSuccess)
                {
                    // Publish CreateProductIntegrationEvent
                    CreateProductIntegrationEvent createProductIntegrationEvent = new CreateProductIntegrationEvent(createProductResponse.Value.ProductId, createProductRequestDto.Name, createProductRequestDto.InitialHand, correlationId);
                    await _eventBus.PublishAsync(createProductIntegrationEvent, "test");

                    transaction.Commit();
                }

                return Result.Success(createProductResponse.Value.ProductId);
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"Add {createProductRequestDto.Name} product failed. Exception detail:{ex.Message}");

                return Result.Failure<int>($"Add {createProductRequestDto.Name} product failed.");
            }

        }
    }
}
