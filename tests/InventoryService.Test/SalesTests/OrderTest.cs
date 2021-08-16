using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SaleService.Dtos;
using SaleService.Services;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Sale
{
    public class OrderTest : SalesMemoryDatabaseConfig
    {
        private OrderService orderService;

        public OrderTest()
        {
            var productLogger = new Mock<ILogger<ProductService>>();
            var productService = new ProductService(Context, productLogger.Object);

            var orderLogger = new Mock<ILogger<OrderService>>();
            orderService = new OrderService(Context, orderLogger.Object, productService);
        }

        #region GetOrderById

        [Fact]
        public async Task GetOrderById_When_OrderId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var order = await orderService.GetOrderByIdAsync(id);

            //Assert
            Assert.True(order.IsFailure);
        }

        [Fact]
        public async Task GetOrderById_When_OrderId_Is_Valid_Return_Order()
        {
            //Arrange
            var id = 1;

            //Act
            var order = await orderService.GetOrderByIdAsync(id);

            //Assert
            Assert.Equal(id, order.Value.Id);
        }

        #endregion

        #region CreateOrder

        [Fact]
        public async Task CreateOrder_When_Order_Is_null_Return_Failure()
        {
            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(null);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Product_Id_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderDto = new CreateOrderDto
            {
                ProductId = 0
            };

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderDto);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Order_Count_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderDto = new CreateOrderDto
            {
                ProductId = 1,
                Count = 0
            };

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderDto);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Product_Id_Is_Invalid_Return_Failure()
        {
            //Arrange
            var createOrderDto = new CreateOrderDto
            {
                ProductId = 12,
                Count = 10
            };

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderDto);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Order_Count_More_Than_Product_Count_Return_Failure()
        {
            //Arrange
            var createOrderDto = new CreateOrderDto
            {
                ProductId = 1,
                Count = 100
            };

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderDto);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Order_Input_Is_Valid_Return_CreateOrderResponseDto()
        {
            //Arrange
            var createOrderDto = new CreateOrderDto
            {
                ProductId = 1,
                Count = 2
            };

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderDto);

            //Assert
            Assert.True(createOrderResponseDto.IsSuccess);
        }

        #endregion

        #region DeleteOrder

        [Fact]
        public async Task DeleteOrder_When_Order_Is_Zero_Return_Failure()
        {
            //Arrange
            int orderId = 0;

            //Act
            var result = await orderService.DeleteOrderAsync(orderId);

            //Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task DeleteOrder_When_Order_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            int orderId = 30;

            //Act
            var createOrderResponseDto = await orderService.DeleteOrderAsync(orderId);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }


        [Fact]
        public async Task DeleteOrder_When_Order_Is_Valid_Return_Success()
        {
            //Arrange
            int orderId = 1;

            //Act
            var UpdateOrderCount = await orderService.DeleteOrderAsync(orderId);

            //Assert
            Assert.True(UpdateOrderCount.IsSuccess);
        }
        #endregion

    }
}
