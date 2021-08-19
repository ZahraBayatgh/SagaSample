using CSharpFunctionalExtensions;
using SalesService.Dtos;
using System.Threading.Tasks;

namespace SalesService.Services
{
    public interface IOrderService
    {
        Task<Result<GetOrderResponse>> GetOrderByIdAsync(int orderId);
        Task<Result<int>> CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto);
        Task<Result<CreateOrderItemResponseDto>> CreateOrderItemAsync(CreateOrderItemRequestDto createOrderItemRequestDto);
        Task<Result> DeleteOrderAsync(int orderId);
    }
}