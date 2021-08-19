using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using SalesService.Dtos;
using SalesService.Services;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Sale
{
    public class OrderTest : SalesMemoryDatabaseConfig
    {
        private OrderService orderService;

        public OrderTest()
        {
            var customerLogger = new Mock<ILogger<CustomerService>>();
            var customerService = new CustomerService(Context, customerLogger.Object);

            var orderLogger = new Mock<ILogger<OrderService>>();
            orderService = new OrderService(Context, orderLogger.Object,  customerService);
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
        public async Task GetOrderById_When_OrderId_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            var id = 2;

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
            Assert.Equal(id, order.Value.CustomerId);
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
        public async Task CreateOrder_When_Customer_Id_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderRequestDto = new CreateOrderRequestDto(0);

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderRequestDto);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Order_Customer_Is_Invalid_Return_Failure()
        {
            //Arrange
            var createOrderRequestDto = new CreateOrderRequestDto(3);

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderRequestDto);

            //Assert
            Assert.True(createOrderResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrder_When_Order_Input_Is_Valid_Return_CreateOrderResponseDto()
        {
            //Arrange
            var createOrderRequestDto = new CreateOrderRequestDto(1);

            //Act
            var createOrderResponseDto = await orderService.CreateOrderAsync(createOrderRequestDto);

            //Assert
            Assert.True(createOrderResponseDto.IsSuccess);
        }

        #endregion

        #region CreateOrderItem

        [Fact]
        public async Task CreateOrderItem_When_OrderItem_Is_null_Return_Failure()
        {
            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(null);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Order_Id_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(0, 1, 2, 200);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Order_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(10, 1, 2, 200);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Product_Id_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(1, 0, 2, 200);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Product_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(10, 10, 2, 200);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Quantity_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(1, 1, 0, 200);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Quantity_Is_More_Then_Than_Product_Count_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(1, 1, 100, 200);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_UnitPrice_Is_Zero_Return_Failure()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(1, 1, 2, 0);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateOrderItem_When_Order_Input_Is_Valid_Return_CreateOrderItemResponseDto()
        {
            //Arrange
            var createOrderItemRequestDto = new CreateOrderItemRequestDto(1, 1, 2, 100);

            //Act
            var createOrderItemResponseDto = await orderService.CreateOrderItemAsync(createOrderItemRequestDto);

            //Assert
            Assert.True(createOrderItemResponseDto.IsSuccess);
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

        #region DeleteOrderItem

        [Fact]
        public async Task DeleteOrderItem_When_OrderItem_Is_Zero_Return_Failure()
        {
            //Arrange
            int orderItemId = 0;

            //Act
            var result = await orderService.DeleteOrderItemAsync(orderItemId);

            //Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task DeleteOrderItem_When_OrderItem_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            int orderItemId = 30;

            //Act
            var createOrderItemResponseDto = await orderService.DeleteOrderItemAsync(orderItemId);

            //Assert
            Assert.True(createOrderItemResponseDto.IsFailure);
        }


        [Fact]
        public async Task DeleteOrderItem_When_OrderItem_Is_Valid_Return_Success()
        {
            //Arrange
            int orderItemId = 1;

            //Act
            var UpdateOrderItemCount = await orderService.DeleteOrderItemAsync(orderItemId);

            //Assert
            Assert.True(UpdateOrderItemCount.IsSuccess);
        }
        #endregion
    }
}
