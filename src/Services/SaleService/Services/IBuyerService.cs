using CSharpFunctionalExtensions;
using SaleService.Dtos;
using SaleService.Models;
using System.Threading.Tasks;

namespace CustomerService.Services
{
    public interface IBuyerService
    {
        Task<Result<int>> CreateBuyerAsync(CreateBuyerRequestDto createBuyerRequest);
        Task<Result<Buyer>> GetBuyerByIdAsync(int buyerId);
    }
}