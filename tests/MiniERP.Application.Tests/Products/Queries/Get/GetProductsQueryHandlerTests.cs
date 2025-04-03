using FluentAssertions;

using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Dtos;
using MiniERP.Application.Products.Queries.Get;
using MiniERP.Products.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Products.Queries.Get;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IRepository<Product>> _mockProductRepository;
    private readonly Mock<IRepository<Category>> _mockCategoryRepository;
    private readonly Mock<IMapper<Product, ProductDto>> _mockProductMapper;
    private readonly Mock<IMapper<Category, CategoryDto>> _mockCategoryMapper;
    private readonly GetProductsQueryHandler _handler;

    public GetProductsQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IRepository<Product>>();
        _mockCategoryRepository = new Mock<IRepository<Category>>();
        _mockProductMapper = new Mock<IMapper<Product, ProductDto>>();
        _mockCategoryMapper = new Mock<IMapper<Category, CategoryDto>>();
        _handler = new GetProductsQueryHandler(
            _mockProductRepository.Object,
            _mockCategoryRepository.Object,
            _mockProductMapper.Object,
            _mockCategoryMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProducts_WhenProductsExist()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Test Product 1", Description = "Test Description 1", UnitPrice = 10.0m, CategoryId = 1 },
            new() { Id = 2, Name = "Test Product 2", Description = "Test Description 2", UnitPrice = 20.0m, CategoryId = 2 }
        };
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Test Category 1" },
            new() { Id = 2, Name = "Test Category 2" }
        };
        var productDtos = new List<ProductDto>
        {
            new() { Id = 1, Name = "Test Product 1", Description = "Test Description 1", UnitPrice = 10.0m, Category = new CategoryDto { Id = 1, Name = "Test Category 1" } },
            new() { Id = 2, Name = "Test Product 2", Description = "Test Description 2", UnitPrice = 20.0m, Category = new CategoryDto { Id = 2, Name = "Test Category 2" } }
        };
        var query = new GetProductsQuery();

        _mockProductRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(products.AsEnumerable()));
        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken ct) => Result.Ok(categories.First(c => c.Id == id)));
        _mockProductMapper.Setup(m => m.Map(It.IsAny<Product>())).Returns((Product product) => productDtos.First(dto => dto.Id == product.Id));
        _mockCategoryMapper.Setup(m => m.Map(It.IsAny<Category>())).Returns((Category category) => productDtos.First(dto => dto.Category.Id == category.Id).Category);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(productDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenNoProductsExist()
    {
        // Arrange
        var query = new GetProductsQuery();

        _mockProductRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("No products found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Test Product 1", Description = "Test Description 1", UnitPrice = 10.0m, CategoryId = 1 }
        };
        var query = new GetProductsQuery();

        _mockProductRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(products.AsEnumerable()));
        _mockCategoryRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Category not found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }
}


