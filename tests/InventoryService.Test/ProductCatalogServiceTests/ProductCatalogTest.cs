using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalogService.Services;
using SagaPattern.UnitTests.Config;
using SaleService.Dtos;
using System.Threading.Tasks;
using Xunit;

namespace SagaPattern.UnitTests.ProductCatalogServiceTests
{
    public class ProductCatalogTest : ProductCatalogMemoryDatabaseConfig
    {
        private ProductService productService;

        public ProductCatalogTest()
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
            var productId = await productService.CreateProductAsync(null);

            //Assert
            Assert.True(productId.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var createProductRequestDto = new ProductCatalog.Dtos.CreateProductRequestDto(null, null, 10);

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductRequestDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var createProductRequestDto = new ProductCatalog.Dtos.CreateProductRequestDto("", null, 10);

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductRequestDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }


        [Fact]
        public async Task CreateProduct_When_Product_Is_Valid_Return_ProductId()
        {
            //Arrange
            var createProductRequestDto = new ProductCatalog.Dtos.CreateProductRequestDto("Pen", null, 10);

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductRequestDto);

            //Assert
            Assert.Equal(7, createProductResponseDto.Value.ProductId);
        }

        #endregion

        #region UpdateProductStatus

        [Fact]
        public async Task UpdateProductStatus_When_UpdateProductStatusDto_Is_null_Return_Failure()
        {
            //Act
            var UpdateProductStatus = await productService.UpdateProductStatusAsync(null);

            //Assert
            Assert.True(UpdateProductStatus.IsFailure);
        }

        [Fact]
        public async Task UpdateProductStatus_When_UpdateProductStatusDto_Name_Is_Null_Return_Failure()
        {
            //Arrange
            var updateProductStatusDto = new UpdateProductStatusRequestDto(null, 1);

            //Act
            var updateProductStatusResult = await productService.UpdateProductStatusAsync(updateProductStatusDto);

            //Assert
            Assert.True(updateProductStatusResult.IsFailure);
        }

        [Fact]
        public async Task UpdateProductStatus_When_UpdateProductStatusDto_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var updateProductStatusDto = new UpdateProductStatusRequestDto("", 1);

            //Act
            var updateProductStatusResult = await productService.UpdateProductStatusAsync(updateProductStatusDto);

            //Assert
            Assert.True(updateProductStatusResult.IsFailure);
        }

        [Fact]
        public async Task UpdateProductStatus_When_UpdateProductStatusDto_ProductStatus_Is_Invalid_Return_Failure()
        {
            //Arrange
            var updateProductStatusDto = new UpdateProductStatusRequestDto("Pen", 2);

            //Act
            var updateProductStatusResult = await productService.UpdateProductStatusAsync(updateProductStatusDto);

            //Assert
            Assert.True(updateProductStatusResult.IsFailure);
        }

        [Fact]
        public async Task UpdateProductStatus_When_Product_Not_Found_Return_Failure()
        {
            //Arrange
            var updateProductStatusDto = new UpdateProductStatusRequestDto("Pen3", 2);

            //Act
            var updateProductStatusResult = await productService.UpdateProductStatusAsync(updateProductStatusDto);

            //Assert
            Assert.True(updateProductStatusResult.IsFailure);
        }

        [Fact]
        public async Task UpdateProductStatus_When_Everything_Is_Ok_Return_Success()
        {
            //Arrange
            var updateProductStatusDto = new UpdateProductStatusRequestDto("Monitor", 2);

            //Act
            var updateProductStatusResult = await productService.UpdateProductStatusAsync(updateProductStatusDto);

            //Assert
            Assert.True(updateProductStatusResult.IsSuccess);
        }

        #endregion

        #region DeleteProduct

        [Fact]
        public async Task DeleteProduct_When_Product_Is_Zero_Return_Failure()
        {
            //Arrange
            int productId = 0;

            //Act
            var result = await productService.DeleteProductAsync(productId);

            //Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task DeleteProduct_When_Product_Is_Not_In_Db_Return_Failure()
        {
            //Arrange
            int productId = 30;

            //Act
            var createProductResponseDto = await productService.DeleteProductAsync(productId);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }


        [Fact]
        public async Task DeleteProduct_When_Product_Is_Valid_Return_Success()
        {
            //Arrange
            int productId = 1;

            //Act
            var UpdateProductCount = await productService.DeleteProductAsync(productId);

            //Assert
            Assert.True(UpdateProductCount.IsSuccess);
        }
        #endregion

    }
}
