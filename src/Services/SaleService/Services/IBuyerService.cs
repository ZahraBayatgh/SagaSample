using CSharpFunctionalExtensions;
using SaleService.Dtos;
using SaleService.Models;
using System.Threading.Tasks;

namespace CustomerService.Services
{
    public interface IBuyerService
    {
        Task<Result<int>> CreateBuyerAsync(BuyerDto buyerDto);
        Task<Result<Buyer>> GetBuyerByIdAsync(int buyerId);
    }
}