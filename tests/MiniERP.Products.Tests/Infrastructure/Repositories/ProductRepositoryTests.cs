using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Products.Infrastructure.Repositories;
using MiniERP.Products.Tests.Mocks;

using Moq;

namespace MiniERP.Products.Tests.Infrastructure.Repositories;

public class ProductRepositoryTests
{
    private readonly Mock<ProductContext> _mockContext;
    private Mock<DbSet<Product>> _mockDbSet;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        _mockContext = new Mock<ProductContext>(new DbContextOptions<ProductContext>());
        _mockDbSet = new Mock<DbSet<Product>>();

        _mockContext.Setup(m => m.Products).Returns(_mockDbSet.Object);
        _repository = new ProductRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "p1", Description = "Amazing product!" };
        var data = new List<Product> { product }.AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Product>(data);

        _mockContext.Setup(m => m.Products).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(product);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFail_WhenProductDoesNotExist()
    {
        // Arrange
        var data = new List<Product>().AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Product>(data);

        _mockContext.Setup(m => m.Products).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == $"Product with ID {1} was not found");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "p1", Description = "Amazing product!" },
            new() { Id = 2, Name = "p2", Description = "Excellent product!" }
        };
        var data = products.AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Product>(data);

        _mockContext.Setup(m => m.Products).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(products);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnSuccess_WhenProductIsAdded()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "p1", Description = "Amazing product!" };


        // Act
        var result = await _repository.AddAsync(product, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockDbSet.Verify(m => m.Add(product), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenProductIsUpdated()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "p1", Description = "Amazing product!" };
        _mockContext.Setup(m => m.Products).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.UpdateAsync(product, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Update(product), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenProductIsDeleted()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "p1", Description = "Amazing product!" };

        _mockContext.Setup(m => m.Products.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _repository.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Products.Remove(product), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFail_WhenProductDoesNotExist()
    {
        // Arrange
        var data = new List<Product>().AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Product>(data);


        _mockContext.Setup(m => m.Products.FindAsync(It.IsAny<int>()))
             .ReturnsAsync(null as Product);

        // Act
        var result = await _repository.DeleteAsync(999, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Product not found");
    }
}

