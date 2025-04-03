using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Domain.Enums;
using MiniERP.Orders.Infrastructure.Database;

namespace MiniERP.Orders.Tests.Infrastructure.Data;

public class OrderDbContextTests : IDisposable
{
    private readonly DbContextOptions<OrderContext> _options;

    public OrderDbContextTests()
    {
        _options = new DbContextOptionsBuilder<OrderContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task OrderTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new OrderContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(Order));

        // Assert
        entityType.GetTableName().Should().Be(nameof(Order));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey.Properties.Single().Name
            .Should().Be(nameof(Order.Id));

        var orderNumberProperty = entityType.FindProperty(nameof(Order.OrderNumber));
        orderNumberProperty.Should().NotBeNull();
        orderNumberProperty.IsNullable
            .Should().BeFalse();

        var dateProperty = entityType.FindProperty(nameof(Order.Date));
        dateProperty.Should().NotBeNull();
        dateProperty.IsNullable
            .Should().BeFalse();

        var userIdProperty = entityType.FindProperty(nameof(Order.UserId));
        userIdProperty.Should().NotBeNull();
        userIdProperty.IsNullable
            .Should().BeFalse();

        var statusProperty = entityType.FindProperty(nameof(Order.Status));
        statusProperty.Should().NotBeNull();
        statusProperty.IsNullable
            .Should().BeFalse();
    }

    [Fact]
    public async Task OrderLineTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new OrderContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(OrderLine));

        //Assert

        entityType.GetTableName()
            .Should().Be(nameof(OrderLine));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey.Properties.Single().Name
            .Should().Be(nameof(OrderLine.Id));

        var productIdProperty = entityType.FindProperty(nameof(OrderLine.ProductId));
        productIdProperty.Should().NotBeNull();
        productIdProperty.IsNullable
            .Should().BeFalse();

        var quantityProperty = entityType.FindProperty(nameof(OrderLine.Quantity));
        quantityProperty.Should().NotBeNull();
        quantityProperty.IsNullable
            .Should().BeFalse();

        var priceProperty = entityType.FindProperty(nameof(OrderLine.Price));
        priceProperty.Should().NotBeNull();
        priceProperty.IsNullable
            .Should().BeFalse();

        var foreignKey = entityType.GetForeignKeys().SingleOrDefault();
        foreignKey.Should().NotBeNull();
        foreignKey.Properties.Single().Name
            .Should().Be(nameof(OrderLine.OrderId));
        foreignKey.DeleteBehavior
            .Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public async Task DeliveryAddressTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new OrderContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(DeliveryAddress));

        // Assert
        entityType.GetTableName().Should().Be(nameof(DeliveryAddress));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey.Properties.Single().Name
            .Should().Be(nameof(DeliveryAddress.Id));

        var streetProperty = entityType.FindProperty(nameof(DeliveryAddress.Street));
        streetProperty.Should().NotBeNull();
        streetProperty.IsNullable
            .Should().BeFalse();

        var cityProperty = entityType.FindProperty(nameof(DeliveryAddress.City));
        cityProperty.Should().NotBeNull();
        cityProperty.IsNullable
            .Should().BeFalse();

        var stateProperty = entityType.FindProperty(nameof(DeliveryAddress.State));
        stateProperty.Should().NotBeNull();
        stateProperty.IsNullable
            .Should().BeFalse();

        var postalCodeProperty = entityType.FindProperty(nameof(DeliveryAddress.PostalCode));
        postalCodeProperty.Should().NotBeNull();
        postalCodeProperty.IsNullable
            .Should().BeFalse();

        var countryProperty = entityType.FindProperty(nameof(DeliveryAddress.Country));
        countryProperty.Should().NotBeNull();
        countryProperty.IsNullable
            .Should().BeFalse();

        var foreignKey = entityType.GetForeignKeys().SingleOrDefault();
        foreignKey.Should().NotBeNull();
        foreignKey.Properties.Single().Name
            .Should().Be(nameof(DeliveryAddress.OrderId));
        foreignKey.DeleteBehavior
            .Should().Be(DeleteBehavior.Cascade);
    }

    [Fact]
    public async Task Order_WhenCreatedWithDeliveryAddress_ShouldSaveCorrectly()
    {
        await using var context = new OrderContext(_options);

        // Arrange
        var order = new Order
        {
            OrderNumber = "12345",
            Date = DateTime.UtcNow,
            UserId = 1,
            Status = OrderStatus.Pending,
            DeliveryAddress = new DeliveryAddress
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "Anystate",
                PostalCode = "12345",
                Country = "USA"
            }
        };

        // Act
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Assert
        var savedOrder = await context.Orders.Include(o => o.DeliveryAddress).FirstOrDefaultAsync(o => o.Id == order.Id);
        savedOrder.Should().NotBeNull();
        savedOrder.DeliveryAddress.Should().NotBeNull();
        savedOrder.DeliveryAddress.Street.Should().Be("123 Main St");
    }

    [Fact]
    public async Task Order_WhenDeleted_ShouldCascadeDeleteDeliveryAddress()
    {
        await using var context = new OrderContext(_options);

        // Arrange
        var order = new Order
        {
            OrderNumber = "12345",
            Date = DateTime.UtcNow,
            UserId = 1,
            Status = OrderStatus.Pending,
            DeliveryAddress = new DeliveryAddress
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "Anystate",
                PostalCode = "12345",
                Country = "USA"
            }
        };
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        var deliveryAddressId = order.DeliveryAddress.Id;

        // Act
        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        // Assert
        var savedOrder = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        var savedDeliveryAddress = await context.DeliveryAddresses.FirstOrDefaultAsync(da => da.OrderId == order.Id);

        savedOrder.Should().BeNull();
        savedDeliveryAddress.Should().BeNull();
    }

    [Fact]
    public async Task Order_WhenCreatedWithOrderLines_ShouldSaveCorrectly()
    {
        await using var context = new OrderContext(_options);

        // Arrange
        var order = new Order
        {
            OrderNumber = "12345",
            Date = DateTime.UtcNow,
            UserId = 1,
            Status = OrderStatus.Pending,
            Lines =
            [
                new() { ProductId = 1, Quantity = 2, Price = 10.0m },
                new() { ProductId = 2, Quantity = 1, Price = 20.0m }
            ]
        };

        // Act
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Assert
        var savedOrder = await context.Orders.Include(o => o.Lines).FirstOrDefaultAsync(o => o.Id == order.Id);
        savedOrder.Should().NotBeNull();
        savedOrder.Lines.Should().HaveCount(2);
    }

    [Fact]
    public async Task Order_WhenDeleted_ShouldCascadeDeleteOrderLines()
    {
        await using var context = new OrderContext(_options);

        // Arrange
        var order = new Order
        {
            OrderNumber = "1234",
            Date = DateTime.UtcNow,
            UserId = 1,
            Status = OrderStatus.Pending,
            Lines =
            [
                new() { ProductId = 1, Quantity = 2, Price = 10.0m },
                new() { ProductId = 2, Quantity = 1, Price = 20.0m }
            ]
        };
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Act
        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        // Assert
        var savedOrder = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        var savedOrderLines = await context.OrderLines.Where(ol => ol.OrderId == order.Id).ToListAsync();
        savedOrder.Should().BeNull();
        savedOrderLines.Should().BeEmpty();
    }

    [Fact]
    public async Task Order_WhenCreated_ShouldHaveDefaultStatus()
    {
        await using var context = new OrderContext(_options);

        // Arrange
        var order = new Order
        {
            OrderNumber = "12345",
            Date = DateTime.UtcNow,
            UserId = 1,
            DeliveryAddress = new DeliveryAddress
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "Anystate",
                PostalCode = "12345",
                Country = "USA"
            }
        };

        // Act
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        // Assert
        var savedOrder = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        savedOrder.Should().NotBeNull();
        savedOrder.Status.Should().Be(OrderStatus.Created);
    }

    public void Dispose()
    {
        // Ensure the database is deleted after each test
        using var context = new OrderContext(_options);
        context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}


