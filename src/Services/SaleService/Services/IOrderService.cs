using CSharpFunctionalExtensions;
using SaleService.Dtos;
using SaleService.Models;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public interface IOrderService
    {
        Task<Result<Order>> GetOrderByIdAsync(int orderId);
        Task<Result<CreateOrderResponseDto>> CreateOrderAsync(CreateOrderDto orderDto);
        Task<Result> DeleteOrderAsync(int orderId);
    }
}