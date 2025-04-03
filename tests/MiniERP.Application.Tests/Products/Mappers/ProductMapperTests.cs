using FluentAssertions;

using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Products.Mappers;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Tests.Products.Mappers
{
    public class ProductMapperTests
    {
        private readonly ProductMapper _mapper;

        public ProductMapperTests()
        {
            _mapper = new ProductMapper();
        }

        [Fact]
        public void Map_ShouldThrowArgumentException_WhenProductDtoIsNull()
        {
            // Act
            Action act = () => _mapper.Map(null as ProductDto);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("ProductDto cannot be null");
        }

        [Fact]
        public void Map_ShouldThrowArgumentException_WhenProductIsNull()
        {
            // Act
            Action act = () => _mapper.Map(null as Product);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Product cannot be null");
        }

        [Fact]
        public void Map_ShouldMapProductDtoToProduct()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Product1",
                Description = "Description1",
                UnitPrice = 100.0m,
                Category = new CategoryDto { Id = 1, Name = "Category1" }
            };

            // Act
            var product = _mapper.Map(productDto);

            // Assert
            product.Should().NotBeNull();
            product.Id.Should().Be(productDto.Id);
            product.Name.Should().Be(productDto.Name);
            product.Description.Should().Be(productDto.Description);
            product.UnitPrice.Should().Be(productDto.UnitPrice);
            product.CategoryId.Should().Be(productDto.Category.Id);
        }

        [Fact]
        public void Map_ShouldMapProductToProductDto()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Product1",
                Description = "Description1",
                UnitPrice = 100.0m,
                CategoryId = 1
            };

            // Act
            var productDto = _mapper.Map(product);

            // Assert
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(product.Id);
            productDto.Name.Should().Be(product.Name);
            productDto.Description.Should().Be(product.Description);
            productDto.UnitPrice.Should().Be(product.UnitPrice);
            productDto.Category.Id.Should().Be(product.CategoryId);
        }
    }
}
