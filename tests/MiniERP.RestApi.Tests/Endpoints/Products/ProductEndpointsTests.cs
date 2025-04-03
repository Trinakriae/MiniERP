using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using MiniERP.Application.Products.Dtos;
using MiniERP.RestApi.Models.Products;

namespace MiniERP.RestApi.Tests.Endpoints.Products
{
    public class ProductEndpointsTests : TestBase
    {
        [Fact]
        public async Task CreateProduct_ShouldReturnCreated_WhenSuccessful()
        {
            var createProductRequest = new CreateProductRequest(
                "Product Name",
                "Product Description",
                10.0m,
                1
            );

            // Act
            var response = await Client.PostAsJsonAsync("/api/products", createProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseObject = await response.Content.ReadFromJsonAsync<int>();
            responseObject.Should().Be(4);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Act
            var response = await Client.GetAsync("/api/products/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            product.Should().NotBeNull();
            product.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOk_WhenProductsExist()
        {
            // Act
            var response = await Client.GetAsync("/api/products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            products.Should().NotBeNull();
            products.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNoContent_WhenSuccessful()
        {
            var updateProductRequest = new UpdateProductRequest(
                 1,
                 "Product Name",
                 "Product Description",
                 10.0m,
                 1
             );

            // Act
            var response = await Client.PutAsJsonAsync("/api/products/1", updateProductRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenSuccessful()
        {
            // Act
            var response = await Client.DeleteAsync("/api/products/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
