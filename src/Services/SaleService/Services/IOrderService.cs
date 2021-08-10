using CSharpFunctionalExtensions;
using SaleService.Dtos;
using System.Threading.Tasks;

namespace SaleService.Services
{
    public interface IOrderService
    {
        Task<Result<CreateOrderDto>> CreateOrderAsync(OrderDto orderDto);
        Task<Result> DeleteOrderAsync(int orderId);
    }
}