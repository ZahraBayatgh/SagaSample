using CSharpFunctionalExtensions;
using CustomerService.Dtos;
using CustomerService.Models;
using System.Threading.Tasks;

namespace CustomerService.Services
{
    public interface IBuyerService
    {
        Task<Result<int>> CreateCustomerAsync(CustomerDto customerDto);
        Task<Result<Customer>> GetCustomerByIdAsync(int customerId);
    }
}