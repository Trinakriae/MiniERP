using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Commands.Create;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Products.Commands.Create
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IRepository<Product>> _mockProductRepository;
        private readonly Mock<IMapper<Product, ProductDto>> _mockProductMapper;
        private readonly Mock<IValidator<CreateProductCommand>> _mockValidator;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IRepository<Product>>();
            _mockProductMapper = new Mock<IMapper<Product, ProductDto>>();
            _mockValidator = new Mock<IValidator<CreateProductCommand>>();
            _handler = new CreateProductCommandHandler(
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
            var command = new CreateProductCommand(productDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockProductMapper.Setup(m => m.Map(productDto)).Returns(product);
            _mockProductRepository.Setup(r => r.AddAsync(product, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());

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
            var command = new CreateProductCommand(productDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult([new ValidationFailure("Name", "Invalid")]));

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
            var command = new CreateProductCommand(productDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ValidationResult([new ValidationFailure("CategoryId", "Category ID does not exist.")]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}
