using CSharpFunctionalExtensions;
using SalesService.Dtos;
using SalesService.Models;
using System.Threading.Tasks;

namespace SalesService.Services
{
    public interface ICustomerService
    {
        Task<Result<int>> CreateCustomerAsync(CreateCustomerRequestDto createCustomerRequest);
        Task<Result<Customer>> GetCustomerByIdAsync(int customerId);
    }
}