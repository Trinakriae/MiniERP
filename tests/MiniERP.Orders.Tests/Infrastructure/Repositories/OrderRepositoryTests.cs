using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Infrastructure.Database;
using MiniERP.Orders.Infrastructure.Repositories;
using MiniERP.Orders.Tests.Mocks;

using Moq;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiniERP.Orders.Tests.Infrastructure.Repositories;

public class OrderRepositoryTests
{
    private readonly Mock<OrderContext> _mockContext;
    private Mock<DbSet<Order>> _mockDbSet;
    private readonly OrderRepository _repository;

    public OrderRepositoryTests()
    {
        _mockContext = new Mock<OrderContext>(new DbContextOptions<OrderContext>());
        _mockDbSet = new Mock<DbSet<Order>>();

        _mockContext.Setup(m => m.Orders).Returns(_mockDbSet.Object);
        _repository = new OrderRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var order = new Order { Id = 1, OrderNumber = "123", Date = DateTime.UtcNow };
        var data = new List<Order> { order }.AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Order>(data);

        _mockContext.Setup(m => m.Orders).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(order);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFail_WhenOrderDoesNotExist()
    {
        // Arrange
        var data = new List<Order>().AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Order>(data);

        _mockContext.Setup(m => m.Orders).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == $"Order with ID {1} was not found");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { Id = 1, OrderNumber = "123", Date = DateTime.UtcNow },
            new() { Id = 2, OrderNumber = "456", Date = DateTime.UtcNow }
        }.AsQueryable();
        var data = orders.AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Order>(data);

        _mockContext.Setup(m => m.Orders).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(orders);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnSuccess_WhenOrderIsAdded()
    {
        // Arrange
        var order = new Order { Id = 1, OrderNumber = "123", Date = DateTime.UtcNow };

        // Act
        var result = await _repository.AddAsync(order, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockDbSet.Verify(m => m.Add(order), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenOrderIsUpdated()
    {
        // Arrange
        var existingOrder = new Order
        {
            Id = 1,
            OrderNumber = "123",
            Date = DateTime.UtcNow,
            DeliveryAddress = new DeliveryAddress
            {
                Id = 1,
                Street = "Street",
                City = "City",
                State = "State",
                PostalCode = "PostalCode",
                Country = "Country",
                OrderId = 1
            },
            Lines = new List<OrderLine>
            {
                new OrderLine { Id = 1, ProductId = 1, Quantity = 1, Price = 100, OrderId = 1 }
            }
        };
        var data = new List<Order> { existingOrder }.AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Order>(data);
        _mockContext.Setup(m => m.Orders).Returns(mockSet.Object);

        var updatedOrder = new Order
        {
            Id = 1,
            OrderNumber = "1234",
            Date = DateTime.UtcNow,
            DeliveryAddress = new DeliveryAddress
            {
                Id = 1,
                Street = "Updated",
                City = "City",
                State = "State",
                PostalCode = "PostalCode",
                Country = "Country",
                OrderId = 1
            },
            Lines = new List<OrderLine>
            {
                new OrderLine { Id = 1, ProductId = 1, Quantity = 1, Price = 100, OrderId = 1 }
            }
        };
       
        // Act
        var result = await _repository.UpdateAsync(updatedOrder, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Update(updatedOrder), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenOrderIsDeleted()
    {
        // Arrange
        var order = new Order { Id = 1, OrderNumber = "123", Date = DateTime.UtcNow };

        _mockContext.Setup(m => m.Orders.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        // Act
        var result = await _repository.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Orders.Remove(order), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFail_WhenOrderDoesNotExist()
    {
        // Arrange
        var data = new List<Order>().AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Order>(data);

        _mockContext.Setup(m => m.Orders.FindAsync(It.IsAny<int>()))
              .ReturnsAsync(null as Order);

        // Act
        var result = await _repository.DeleteAsync(999, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Order not found");
    }

}

