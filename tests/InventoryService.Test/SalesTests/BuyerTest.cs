using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SalesService.Dtos;
using SalesService.Services;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class CustomerTest : SalesMemoryDatabaseConfig
    {
        private CustomerService customerService;

        public CustomerTest()
        {
            var logger = new Mock<ILogger<CustomerService>>();

            customerService = new CustomerService(Context, logger.Object);
        }

        #region GetCustomerById

        [Fact]
        public async Task GetCustomerById_When_CustomerId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var customer = await customerService.GetCustomerByIdAsync(id);

            //Assert
            Assert.True(customer.IsFailure);
        }

        [Fact]
        public async Task GetCustomerById_When_CustomerId_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            var id = 2;

            //Act
            var customer = await customerService.GetCustomerByIdAsync(id);

            //Assert
            Assert.True(customer.IsFailure);
        }

        [Fact]
        public async Task GetCustomerById_When_CustomerId_Is_Valid_Return_Customer()
        {
            //Arrange
            var id = 1;

            //Act
            var customer = await customerService.GetCustomerByIdAsync(id);

            //Assert
            Assert.Equal(id, customer.Value.Id);
        }

        #endregion

        #region CreateCustomer

        [Fact]
        public async Task CreateCustomer_When_Customer_Is_null_Return_Failure()
        {
            //Act
            var createCustomerResponseDto = await customerService.CreateCustomerAsync(null);

            //Assert
            Assert.True(createCustomerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateCustomer_When_FirstName_Is_Null_Return_Failure()
        {
            //Arrange
            var createCustomerRequestDto = new CreateCustomerRequestDto(null, "Bayat");

            //Act
            var createCustomerResponseDto = await customerService.CreateCustomerAsync(createCustomerRequestDto);

            //Assert
            Assert.True(createCustomerResponseDto.IsFailure);
        }
        [Fact]
        public async Task CreateCustomer_When_FirstName_Is_Empty_Return_Failure()
        {
            //Arrange
            var createCustomerRequestDto = new CreateCustomerRequestDto("", "Bayat");

            //Act
            var createCustomerResponseDto = await customerService.CreateCustomerAsync(createCustomerRequestDto);

            //Assert
            Assert.True(createCustomerResponseDto.IsFailure);
        }
        [Fact]
        public async Task CreateCustomer_When_LasttName_Is_Null_Return_Failure()
        {
            //Arrange
            var createCustomerRequestDto = new CreateCustomerRequestDto("Zahra", null);

            //Act
            var createCustomerResponseDto = await customerService.CreateCustomerAsync(createCustomerRequestDto);

            //Assert
            Assert.True(createCustomerResponseDto.IsFailure);
        }
        [Fact]
        public async Task CreateCustomer_When_LastName_Is_Null_Return_Failure()
        {
            //Arrange
            var createCustomerRequestDto = new CreateCustomerRequestDto("Zahra", "");

            //Act
            var createCustomerResponseDto = await customerService.CreateCustomerAsync(createCustomerRequestDto);

            //Assert
            Assert.True(createCustomerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateCustomer_When_Customer_Input_Is_Valid_Return_CreateCustomerResponseDto()
        {
            //Arrange
            var createCustomerRequestDto = new CreateCustomerRequestDto("Ali", "Bayat");

            //Act
            var createCustomerResponseDto = await customerService.CreateCustomerAsync(createCustomerRequestDto);

            //Assert
            Assert.True(createCustomerResponseDto.IsSuccess);
        }

        #endregion

    }
}
