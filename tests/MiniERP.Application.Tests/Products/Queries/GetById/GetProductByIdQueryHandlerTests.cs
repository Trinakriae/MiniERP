using FluentAssertions;

using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Products.Queries.GetById;
using MiniERP.Products.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Products.Queries.GetById;

public class GetProductByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Product>> _mockProductRepository;
    private readonly Mock<IRepository<Category>> _mockCategoryRepository;
    private readonly Mock<IMapper<Product, ProductDto>> _mockProductMapper;
    private readonly Mock<IMapper<Category, CategoryDto>> _mockCategoryMapper;
    private readonly GetProductByIdQueryHandler _handler;

    public GetProductByIdQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IRepository<Product>>();
        _mockCategoryRepository = new Mock<IRepository<Category>>();
        _mockProductMapper = new Mock<IMapper<Product, ProductDto>>();
        _mockCategoryMapper = new Mock<IMapper<Category, CategoryDto>>();
        _handler = new GetProductByIdQueryHandler(
            _mockProductRepository.Object,
            _mockCategoryRepository.Object,
            _mockProductMapper.Object,
            _mockCategoryMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var categoryId = 1;
        var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, CategoryId = categoryId };
        var category = new Category { Id = categoryId, Name = "Test Category" };
        var productDto = new ProductDto { Id = productId, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, Category = new CategoryDto { Id = categoryId, Name = "Test Category" } };
        var query = new GetProductByIdQuery(productId);

        _mockProductRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(product));
        _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(category));
        _mockProductMapper.Setup(m => m.Map(product)).Returns(productDto);
        _mockCategoryMapper.Setup(m => m.Map(category)).Returns(productDto.Category);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(productDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        var query = new GetProductByIdQuery(productId);

        _mockProductRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Product not found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        var productId = 1;
        var categoryId = 1;
        var product = new Product { Id = productId, Name = "Test Product", Description = "Test Description", UnitPrice = 10.0m, CategoryId = categoryId };
        var query = new GetProductByIdQuery(productId);

        _mockProductRepository.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(product));
        _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Category not found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }
}


