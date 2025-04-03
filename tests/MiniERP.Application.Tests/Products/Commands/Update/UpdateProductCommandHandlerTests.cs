using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Products.Commands.Update;
using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Users.Commands.Update;
using MiniERP.Application.Users.Dtos;
using MiniERP.Products.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Products.Commands.Update
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IMapper<Product, ProductDto>> _mockProductMapper;
        private readonly Mock<IValidator<UpdateProductCommand>> _mockValidator;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockProductMapper = new Mock<IMapper<Product, ProductDto>>();
            _mockValidator = new Mock<IValidator<UpdateProductCommand>>();
            _handler = new UpdateProductCommandHandler(
                _mockProductRepository.Object,
                _mockProductMapper.Object,
                _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenProductIsValid()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, Category = new CategoryDto { Id = 1 } };
            var product = new Product { Id = 1, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, CategoryId = 1 };
            var command = new UpdateProductCommand(productDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockProductMapper.Setup(m => m.Map(productDto)).Returns(product);
            _mockProductRepository.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok());
            _mockProductRepository.Setup(r => r.GetByIdAsync(productDto.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(product));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenProductIsInvalid()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, Category = new CategoryDto { Id = 1 } };
            var command = new UpdateProductCommand(productDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure("ProductDto.Name", "Product name must not be empty."),
                    new ValidationFailure("ProductDto.UnitPrice", "Product price must be greater than zero."),
                    new ValidationFailure("ProductDto.Category", "Product must have a category."),
                    new ValidationFailure("ProductDto.Category.Id", "Category does not exist.")
                ]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCategoryDoesNotExist()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, Category = new CategoryDto { Id = 1 } };
            var product = new Product { Id = 1, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, CategoryId = 1 };
            var command = new UpdateProductCommand(productDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockProductMapper.Setup(m => m.Map(productDto)).Returns(product);
            _mockProductRepository.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());
            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure("ProductDto.Category.Id", "Category does not exist.")
                ]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductIsNotFound()
        {
            // Arrange
            var orderDto = new ProductDto { Id = 1 };
            var command = new UpdateProductCommand(orderDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockProductRepository.Setup(r => r.GetByIdAsync(orderDto.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail(ResultErrors.NotFound<Product>(orderDto.Id.Value)));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }
    }
}
