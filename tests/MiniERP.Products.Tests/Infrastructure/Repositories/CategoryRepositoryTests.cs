using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;
using MiniERP.Products.Infrastructure.Repositories;
using MiniERP.Products.Tests.Mocks;

using Moq;

namespace MiniERP.Products.Tests.Infrastructure.Repositories;

public class CategoryRepositoryTests
{
    private readonly Mock<ProductContext> _mockContext;
    private Mock<DbSet<Category>> _mockDbSet;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        _mockContext = new Mock<ProductContext>(new DbContextOptions<ProductContext>());
        _mockDbSet = new Mock<DbSet<Category>>();

        _mockContext.Setup(m => m.Categories).Returns(_mockDbSet.Object);
        _repository = new CategoryRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Category1" };
        var data = new List<Category> { category }.AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Category>(data);

        _mockContext.Setup(m => m.Categories).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(category);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        var data = new List<Category>().AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Category>(data);

        _mockContext.Setup(m => m.Categories).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == $"Category with ID {1} was not found");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Category1" },
            new() { Id = 2, Name = "Category2" }
        };
        var data = categories.AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Category>(data);

        _mockContext.Setup(m => m.Categories).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnSuccess_WhenCategoryIsAdded()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Category1" };

        // Act
        var result = await _repository.AddAsync(category, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockDbSet.Verify(m => m.Add(category), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenCategoryIsUpdated()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Category1" };
        _mockContext.Setup(m => m.Categories).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.UpdateAsync(category, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Update(category), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenCategoryIsDeleted()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Category1" };

        _mockContext.Setup(m => m.Categories.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _repository.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Categories.Remove(category), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFail_WhenCategoryDoesNotExist()
    {
        // Arrange
        var data = new List<Category>().AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Category>(data);

        _mockContext.Setup(m => m.Categories.FindAsync(It.IsAny<int>()))
             .ReturnsAsync(null as Category);

        // Act
        var result = await _repository.DeleteAsync(999, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Category not found");
    }
}



