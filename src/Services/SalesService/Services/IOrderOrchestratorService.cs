using CSharpFunctionalExtensions;
using SalesService.Dtos;
using System.Threading.Tasks;

namespace SalesService.Services
{
    public interface IOrderOrchestratorService
    {
        Task<Result> AddOrderAndUpdateProduct(CreateOrderItemRequestDto createOrderItemRequestDto, string correlationId);
    }
}