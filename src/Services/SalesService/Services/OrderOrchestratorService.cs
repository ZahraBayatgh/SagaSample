using CSharpFunctionalExtensions;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using SalesService.Data;
using SalesService.Dtos;
using SalesService.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace SalesService.Services
{
    public class OrderOrchestratorService : IOrderOrchestratorService
    {
        private SaleDbContext _context;
        private ILogger<OrderOrchestratorService> _logger;
        private IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IEventBus _eventBus;

        public OrderOrchestratorService(SaleDbContext context,
          ILogger<OrderOrchestratorService> logger,
          IOrderService orderService,
          IProductService productService,
          IEventBus eventBus)
        {
            _context = context;
            _logger = logger;
            _orderService = orderService;
            _productService = productService;
            _eventBus = eventBus;
        }
        public async Task<Result> AddOrderAndUpdateProduct(CreateOrderItemRequestDto createOrderItemRequestDto, string correlationId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var orderResult = await _orderService.CreateOrderItemAsync(createOrderItemRequestDto);

                if (orderResult.IsSuccess)
                {
                    // Intialize UpdateProductCountDto
                    UpdateProductCountDto updateProductCountDto = new UpdateProductCountDto
                    {
                        Name = orderResult.Value.ProductName,
                        Quantity = createOrderItemRequestDto.Quantity
                    };

                    // Update product count in product table
                    var product = await _productService.UpdateProductCountAsync(updateProductCountDto);
                    if (product.IsSuccess)
                    {
                        UpdateInventoryEvent updateProductIntegrationEvent = new UpdateInventoryEvent(updateProductCountDto.Name, updateProductCountDto.Quantity, orderResult.Value.OrderId, orderResult.Value.OrderItemId, correlationId);
                        await _eventBus.PublishAsync(updateProductIntegrationEvent);
                    }
                    transaction.Commit();

                    return Result.Success();
                }
                return Result.Failure($"Add orderItem has been failed.");
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation($"Add orderItem has been failed. Exception detail:{ex.Message}");

                return Result.Failure($"Add orderItem has been failed.");
            }
        }
    }
}
