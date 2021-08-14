
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using SaleService.Dtos;
using SaleService.Services;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.Tests.Sale
{
    public class ProductTest : SaleMemoryDatabaseConfig
    {
        private ProductService productService;

        public ProductTest()
        {
            var logger = new Mock<ILogger<ProductService>>();
            productService = new ProductService(Context, logger.Object);
        }

        #region GetProductById

        [Fact]
        public async Task GetProductById_When_ProductId_Is_Invalid_Return_Failure()
        {
            //Arrange
            var id = 0;

            //Act
            var product = await productService.GetProductByIdAsync(id);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task GetProductById_When_ProductId_Is_Valid_Return_Product()
        {
            //Arrange
            var id = 1;

            //Act
            var product = await productService.GetProductByIdAsync(id);

            //Assert
            Assert.Equal(id, product.Value.Id);
        }

        #endregion

        #region CreateProduct

        [Fact]
        public async Task CreateProduct_When_Product_Is_null_Return_Failure()
        {
            //Act
            var productId = await productService.CreateProductAsync(null);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var createProductDto = new CreateProductDto
            {
                Count = 10
            };

            //Act
            var productId = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var createProductDto = new CreateProductDto
            {
                Name = "",
                Count = 10
            };

            //Act
            var productId = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Count_Is_Zero_Return_Failure()
        {
            //Arrange
            var createProductDto = new CreateProductDto
            {
                Name = "Pen",
                Count = 0
            };

            //Act
            var productId = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Is_Valid_Return_ProductId()
        {
            //Arrange
            var createProductDto = new CreateProductDto
            {
                Name = "Pen",
                Count = 10
            };

            //Act
            var productId = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.Equal(3, productId.Value);
        }

        #endregion

        #region UpdateProductCount

        [Fact]
        public async Task UpdateProductCount_When_UpdateProductCountDto_Is_null_Return_Failure()
        {
            //Act
            var UpdateProductCount = await productService.UpdateProductCountAsync(null);

            //Assert
            Assert.True(UpdateProductCount.IsFailure);
        }

        [Fact]
        public async Task UpdateProductCount_When_UpdateProductCountDto_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var UpdateProductCountDto = new UpdateProductCountDto
            {
                DecreaseCount = 3
            };

            //Act
            var productId = await productService.UpdateProductCountAsync(UpdateProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task UpdateProductCount_When_UpdateProductCountDto_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var UpdateProductCountDto = new UpdateProductCountDto
            {
                Name = "",
                DecreaseCount = 3
            };

            //Act
            var productId = await productService.UpdateProductCountAsync(UpdateProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task UpdateProductCount_When_UpdateProductCountDto_DecreaseCount_Is_Zero_Return_Failure()
        {
            //Arrange
            var updateProductCountDto = new UpdateProductCountDto
            {
                Name = "Mouse",
                DecreaseCount = 0
            };

            //Act
            var productId = await productService.UpdateProductCountAsync(updateProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task UpdateProductCount_When_Product_Count_Is_Lesser_Than_DecreaseCountReturn_Failure()
        {
            //Arrange
            var updateProductCountDto = new UpdateProductCountDto
            {
                Name = "Mouse",
                DecreaseCount = 1000
            };

            //Act
            var productId = await productService.UpdateProductCountAsync(updateProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task UpdateProductCount_When_UpdateProductCountDto_Is_Valid_Return_ProductId()
        {
            //Arrange
            var UpdateProductCountDto = new UpdateProductCountDto
            {
                Name = "Mouse",
                DecreaseCount = 3
            };

            //Act
            var product = await productService.UpdateProductCountAsync(UpdateProductCountDto);

            //Assert
            Assert.True(product.IsSuccess);
        }

        #endregion

        #region CancelChangeProductCount

        [Fact]
        public async Task CancelChangeProductCount_When_UpdateProductCountDto_Is_null_Return_Failure()
        {
            //Act
            var UpdateProductCount = await productService.CancelChangeProductCountAsync(null);

            //Assert
            Assert.True(UpdateProductCount.IsFailure);
        }

        [Fact]
        public async Task CancelChangeProductCount_When_UpdateProductCountDto_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var cancelChangeProductCountDto = new CancelChangeProductCountDto
            {
                DecreaseCount = 3
            };

            //Act
            var productId = await productService.CancelChangeProductCountAsync(cancelChangeProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CancelChangeProductCount_When_UpdateProductCountDto_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var cancelChangeProductCountDto = new CancelChangeProductCountDto
            {
                Name = "",
                DecreaseCount = 3
            };

            //Act
            var productId = await productService.CancelChangeProductCountAsync(cancelChangeProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CancelChangeProductCount_When_UpdateProductCountDto_DecreaseCount_Is_Zero_Return_Failure()
        {
            //Arrange
            var cancelChangeProductCountDto = new CancelChangeProductCountDto
            {
                Name = "Mouse",
                DecreaseCount = 0
            };

            //Act
            var productId = await productService.CancelChangeProductCountAsync(cancelChangeProductCountDto);

            //Assert
            Assert.True(productId.IsFailure);
        }
        [Fact]
        public async Task CancelChangeProductCount_When_UpdateProductCountDto_Is_Valid_Return_Success()
        {
            //Arrange
            var cancelChangeProductCountDto = new CancelChangeProductCountDto
            {
                Name = "Mouse",
                DecreaseCount = 20
            };

            //Act
            var result = await productService.CancelChangeProductCountAsync(cancelChangeProductCountDto);

            //Assert
            Assert.True(result.IsSuccess);
        }

        #endregion
    }
}
