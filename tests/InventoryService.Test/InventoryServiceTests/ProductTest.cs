using InventoryService.Dtos;
using InventoryService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SagaPattern.UnitTests.Config;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.InventoryServiceTests
{
    public class ProductTest : InventoryMemoryDatabaseConfig
    {
        private ProductService productService;

        public ProductTest()
        {
            var logger = new Mock<ILogger<ProductService>>();

            productService = new ProductService(Context, logger.Object);
        }

        #region GetProductById

        [Fact]
        public async Task GetProductById_When_ProducId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var product = await productService.GetProductByIdAsync(id);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task GetProductById_When_Product_Is_Not_Found_Return_Failure()
        {
            //Arrange
            var id = 1000;

            //Act
            var product = await productService.GetProductByIdAsync(id);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task GetProductById_When_ProducId_Is_Valid_Return_Product()
        {
            //Arrange
            var id = 1;

            //Act
            var product = await productService.GetProductByIdAsync(id);

            //Assert
            Assert.Equal(id, product.Value.Id);
        }

        #endregion

        #region GetProductId

        [Fact]
        public async Task GetProductId_When_Name_Is_Invalid_Return_Failure()
        {
            //Arrange
            var name = "";

            //Act
            var product = await productService.GetProductIdAsync(name);

            //Assert
            Assert.True(product.IsFailure);
        }
        [Fact]
        public async Task GetProductId_When_Product_Is_Not_Found_Return_Failure()
        {
            //Arrange
            var id = 1000;

            //Act
            var product = await productService.GetProductByIdAsync(id);

            //Assert
            Assert.True(product.IsFailure);
        }
        [Fact]
        public async Task GetProductId_When_Name_Is_Valid_Return_Product()
        {
            //Arrange
            var name = "Mouse";

            //Act
            var product = await productService.GetProductIdAsync(name);

            //Assert
            Assert.Equal(1, product.Value);
        }

        #endregion

        #region GetProductByName

        [Fact]
        public async Task GetProductByName_When_ProductName_Is_Null_Return_Failure()
        {
            //Act
            var product = await productService.GetProductByNameAsync(null);

            //Assert
            Assert.True(product.IsFailure);
        }
        [Fact]
        public async Task GetProductByName_When_ProductName_Is_Empty_Return_Failure()
        {
            //Act
            var product = await productService.GetProductByNameAsync("");

            //Assert
            Assert.True(product.IsFailure);
        }
        [Fact]
        public async Task GetProductByName_When_Product_Is_Not_Found_Return_Failure()
        {
            //Act
            var product = await productService.GetProductByNameAsync("Clock");

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task GetProductByName_When_ProductId_Is_Valid_Return_Product()
        {
            //Act
            var product = await productService.GetProductByNameAsync("Mouse");

            //Assert
            Assert.Equal(1, product.Value.Id);
        }

        #endregion

        #region CreateProduct

        [Fact]
        public async Task CreateProduct_When_Product_Is_null_Return_Failure()
        {
            //Act
            var createProductResponseDto = await productService.CreateProductAsync(null);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                Count = 10
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(productRequestDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                ProductName = "",
                Count = 10
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(productRequestDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Count_Is_Zero_Return_Failure()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                ProductName = "Pen",
                Count = 0
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(productRequestDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Is_Valid_Return_ProductId()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                ProductName = "Pen",
                Count = 10
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(productRequestDto);

            //Assert
            Assert.Equal(3, createProductResponseDto.Value.ProductId);
        }

        #endregion


        #region UpdateProduct

        [Fact]
        public async Task UpdateProduct_When_Product_Is_null_Return_Failure()
        {
            //Act
            var product = await productService.UpdateProductAsync(null);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                Count = 10
            };

            //Act
            var product = await productService.UpdateProductAsync(productRequestDto);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                ProductName = "",
                Count = 10
            };

            //Act
            var product = await productService.UpdateProductAsync(productRequestDto);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Count_Is_Lesser_than_Zero_Return_Failure()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                ProductName = "Mouse",
                Count = -2
            };

            //Act
            var product = await productService.UpdateProductAsync(productRequestDto);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Is_Valid_Return_Product()
        {
            //Arrange
            var productRequestDto = new ProductRequestDto
            {
                ProductName = "Mouse",
                Count = 10
            };

            //Act
            var product = await productService.UpdateProductAsync(productRequestDto);

            //Assert
            Assert.Equal(10, product.Value.Count);
        }

        #endregion
    }
}
