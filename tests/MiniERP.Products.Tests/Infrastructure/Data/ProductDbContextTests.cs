using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Products.Domain.Entities;
using MiniERP.Products.Infrastructure.Database;

namespace MiniERP.Products.Tests.Infrastructure.Data;

public class ProductContextTests : IDisposable
{
    private readonly DbContextOptions<ProductContext> _options;

    public ProductContextTests()
    {
        _options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task ProductTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new ProductContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(Product));

        // Assert
        entityType.Should().NotBeNull();
        entityType!.GetTableName().Should().Be(nameof(Product));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey!.Properties.Single().Name
            .Should().Be(nameof(Product.Id));

        var nameProperty = entityType.FindProperty(nameof(Product.Name));
        nameProperty.Should().NotBeNull();
        nameProperty!.IsNullable
            .Should().BeFalse();

        var unitPriceProperty = entityType.FindProperty(nameof(Product.UnitPrice));
        unitPriceProperty.Should().NotBeNull();
        unitPriceProperty!.IsNullable
            .Should().BeFalse();
    }

    [Fact]
    public async Task CategoryTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new ProductContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(Category));

        // Assert
        entityType.Should().NotBeNull();
        entityType!.GetTableName().Should().Be(nameof(Category));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey!.Properties.Single().Name
            .Should().Be(nameof(Category.Id));

        var nameProperty = entityType.FindProperty(nameof(Category.Name));
        nameProperty.Should().NotBeNull();
        nameProperty!.IsNullable
            .Should().BeFalse();
    }

    [Fact]
    public async Task Product_WhenCreatedWithCategory_ShouldSaveCorrectly()
    {
        await using var context = new ProductContext(_options);

        // Arrange
        var category = new Category
        {
            Name = "Electronics"
        };

        await context.Categories.AddAsync(category);

        var product = new Product
        {
            Name = "Laptop",
            Description = "A high-end gaming laptop",
            UnitPrice = 1500.00m,
            CategoryId = category.Id,
        };

        // Act
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        savedProduct.Should().NotBeNull();
        savedProduct!.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public async Task Product_WhenDeleted_ShouldNotCascadeDelete()
    {
        await using var context = new ProductContext(_options);

        // Arrange
        var category = new Category
        {
            Name = "Electronics"
        };

        await context.Categories.AddAsync(category);

        var product = new Product
        {
            Name = "Laptop",
            Description = "A high-end gaming laptop",
            UnitPrice = 1500.00m,
            CategoryId = category.Id,
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        // Act
        context.Products.Remove(product);
        await context.SaveChangesAsync();

        // Assert
        var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        var savedCategory = await context.Categories.FirstOrDefaultAsync(c => c.Id == product.CategoryId);
        savedProduct.Should().BeNull();
        savedCategory.Should().NotBeNull();
    }

    public void Dispose()
    {
        using var context = new ProductContext(_options);
        context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}







