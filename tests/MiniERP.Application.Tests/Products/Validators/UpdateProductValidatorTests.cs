using FluentValidation.TestHelper;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Commands.Update;
using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Products.Validators;
using MiniERP.Products.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Products.Validators
{
    public class UpdateProductValidatorTests
    {
        private readonly Mock<IRepository<Category>> _mockCategoryRepository;
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly UpdateProductCommandValidator _validator;

        public UpdateProductValidatorTests()
        {
            _mockCategoryRepository = new Mock<IRepository<Category>>();
            _mockProductRepository = new Mock<IRepository<Product>>();
            _validator = new UpdateProductCommandValidator(_mockCategoryRepository.Object, _mockProductRepository.Object);
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductIsNull()
        {
            // Arrange
            UpdateProductCommand? command = new(null as ProductDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto).WithErrorMessage("Product must not be null.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductIdIsNull()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = null,
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m,
                Category = new CategoryDto { Id = 1 }
            };
            var command = new UpdateProductCommand(productDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto.Id).WithErrorMessage("Product ID must not be null.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductDoesNotExist()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m,
                Category = new CategoryDto { Id = 1 }
            };
            var command = new UpdateProductCommand(productDto);

            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto.Id.Value).WithErrorMessage("Product does not exist.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductNameIsEmpty()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = string.Empty,
                Description = "Test Description",
                UnitPrice = 10.0m,
                Category = new CategoryDto { Id = 1 }
            };
            var command = new UpdateProductCommand(productDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto.Name).WithErrorMessage("Product name must not be empty.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductPriceIsZeroOrLess()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 0,
                Category = new CategoryDto { Id = 1 }
            };
            var command = new UpdateProductCommand(productDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto.UnitPrice).WithErrorMessage("Product price must be greater than zero.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenCategoryIsNull()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m,
                Category = null
            };
            var command = new UpdateProductCommand(productDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto.Category).WithErrorMessage("Product must have a category.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenCategoryDoesNotExist()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m,
                Category = new CategoryDto { Id = 1 }
            };
            var command = new UpdateProductCommand(productDto);

            _mockCategoryRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductDto.Category.Id).WithErrorMessage("Category does not exist.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenProductIsValid()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m,
                Category = new CategoryDto { Id = 1 }
            };
            var command = new UpdateProductCommand(productDto);

            _mockCategoryRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _mockProductRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}


