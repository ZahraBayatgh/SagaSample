using CustomerService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SaleService.Dtos;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.SaleTests
{
    public class BuyerTest : SalesMemoryDatabaseConfig
    {
        private BuyerService buyerService;

        public BuyerTest()
        {
            var logger = new Mock<ILogger<BuyerService>>();

            buyerService = new BuyerService(Context, logger.Object);
        }

        #region GetBuyerById

        [Fact]
        public async Task GetBuyerById_When_BuyerId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var buyer = await buyerService.GetBuyerByIdAsync(id);

            //Assert
            Assert.True(buyer.IsFailure);
        }

        [Fact]
        public async Task GetBuyerById_When_BuyerId_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            var id = 2;

            //Act
            var buyer = await buyerService.GetBuyerByIdAsync(id);

            //Assert
            Assert.True(buyer.IsFailure);
        }

        [Fact]
        public async Task GetBuyerById_When_BuyerId_Is_Valid_Return_Buyer()
        {
            //Arrange
            var id = 1;

            //Act
            var buyer = await buyerService.GetBuyerByIdAsync(id);

            //Assert
            Assert.Equal(id, buyer.Value.Id);
        }

        #endregion

        #region CreateBuyer

        [Fact]
        public async Task CreateBuyer_When_Buyer_Is_null_Return_Failure()
        {
            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(null);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateBuyer_When_FirstName_Is_Null_Return_Failure()
        {
            //Arrange
            var createBuyerRequestDto = new CreateBuyerRequestDto(null, "Bayat");

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerRequestDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }
        [Fact]
        public async Task CreateBuyer_When_FirstName_Is_Empty_Return_Failure()
        {
            //Arrange
            var createBuyerRequestDto = new CreateBuyerRequestDto("", "Bayat");

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerRequestDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }
        [Fact]
        public async Task CreateBuyer_When_LasttName_Is_Null_Return_Failure()
        {
            //Arrange
            var createBuyerRequestDto = new CreateBuyerRequestDto("Zahra", null);

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerRequestDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }
        [Fact]
        public async Task CreateBuyer_When_LastName_Is_Null_Return_Failure()
        {
            //Arrange
            var createBuyerRequestDto = new CreateBuyerRequestDto("Zahra", "");

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerRequestDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateBuyer_When_Buyer_Input_Is_Valid_Return_CreateBuyerResponseDto()
        {
            //Arrange
            var createBuyerRequestDto = new CreateBuyerRequestDto("Ali", "Bayat");

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerRequestDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsSuccess);
        }

        #endregion

    }
}
