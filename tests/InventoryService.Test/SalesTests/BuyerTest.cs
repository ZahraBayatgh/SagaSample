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
        public async Task GetBuyerById_When_ProducId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var buyer = await buyerService.GetBuyerByIdAsync(id);

            //Assert
            Assert.True(buyer.IsFailure);
        }

        [Fact]
        public async Task GetBuyerById_When_ProducId_Is_Valid_Return_Buyer()
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
        public async Task CreateBuyer_When_Buyer_FirstName_Is_Null_Return_Failure()
        {
            //Arrange
            var createBuyerDto = new BuyerDto
            {
                LastName="Bayat"
            };

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateBuyer_When_Buyer_LastName_Is_Empty_Return_Failure()
        {
            //Arrange
            var createBuyerDto = new BuyerDto
            {
                FirstName = "",
                LastName = "Bayat"
            };

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateBuyer_When_Buyer_LastName_Is_Null_Return_Failure()
        {
            //Arrange
            var createBuyerDto = new BuyerDto
            {
                FirstName = "Sara"
            };

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateBuyer_When_Buyer_FirstName_Is_Empty_Return_Failure()
        {
            //Arrange
            var createBuyerDto = new BuyerDto
            {
                FirstName = "Sara",
                LastName = ""
            };

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerDto);

            //Assert
            Assert.True(createBuyerResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateBuyer_When_Buyer_Is_Valid_Return_BuyerId()
        {
            //Arrange
            var createBuyerDto = new BuyerDto
            {
                FirstName = "Sara",
                LastName = "Bayat"
            };

            //Act
            var createBuyerResponseDto = await buyerService.CreateBuyerAsync(createBuyerDto);

            //Assert
            Assert.Equal(2, createBuyerResponseDto.Value);
        }

        #endregion

    }
}
