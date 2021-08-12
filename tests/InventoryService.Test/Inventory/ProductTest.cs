using InventoryService.Dtos;
using InventoryService.Models;
using InventoryService.Services;
using InventoryService.Test.Config;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace InventoryService.Test
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
            var createProductDto = new ProductDto
            {
                Count = 10
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                Name = "",
                Count = 10
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Count_Is_Zero_Return_Failure()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                Name = "Pen",
                Count = 0
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.True(createProductResponseDto.IsFailure);
        }

        [Fact]
        public async Task CreateProduct_When_Product_Is_Valid_Return_ProductId()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                Name = "Pen",
                Count = 10
            };

            //Act
            var createProductResponseDto = await productService.CreateProductAsync(createProductDto);

            //Assert
            Assert.Equal(3, createProductResponseDto.Value.ProductId);
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
            var createProductDto = new ProductDto
            {
                Count = 10
            };

            //Act
            var product = await productService.UpdateProductAsync(createProductDto);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Name_Is_Empty_Return_Failure()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                Name = "",
                Count = 10
            };

            //Act
            var product = await productService.UpdateProductAsync(createProductDto);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Count_Is_Lesser_than_Zero_Return_Failure()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                Name = "Mouse",
                Count = -2
            };

            //Act
            var product = await productService.UpdateProductAsync(createProductDto);

            //Assert
            Assert.True(product.IsFailure);
        }

        [Fact]
        public async Task UpdateProduct_When_Product_Is_Valid_Return_Product()
        {
            //Arrange
            var createProductDto = new ProductDto
            {
                Name = "Mouse",
                Count = 10
            };

            //Act
            var product = await productService.UpdateProductAsync(createProductDto);

            //Assert
            Assert.Equal(10, product.Value.Count);
        }

        #endregion
    }
}
