using FluentAssertions;

using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Queries.GetById;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Queries.GetById;

public class GetOrderByIdQueryHandlerTests
{
    private readonly Mock<IRepository<Order>> _mockOrderRepository;
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper<Order, OrderDto>> _mockOrderMapper;
    private readonly Mock<IMapper<User, OrderUserDto>> _mockUserMapper;
    private readonly GetOrderByIdQueryHandler _handler;

    public GetOrderByIdQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockOrderMapper = new Mock<IMapper<Order, OrderDto>>();
        _mockUserMapper = new Mock<IMapper<User, OrderUserDto>>();
        _handler = new GetOrderByIdQueryHandler(
            _mockOrderRepository.Object,
            _mockUserRepository.Object,
            _mockOrderMapper.Object,
            _mockUserMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrder_WhenOrderExists()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { Id = orderId, OrderNumber = "ORD123", UserId = 1 };
        var orderDto = new OrderDto { Id = orderId, OrderNumber = "ORD123", User = new OrderUserDto { Id = 1 } };
        var user = new User { Id = 1, FirstName = "John", LastName = "Doe" };
        var userSummaryDto = new OrderUserDto { Id = 1, FirstName = "John", LastName = "Doe" };
        var query = new GetOrderByIdQuery(orderId);

        _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(order));
        _mockOrderMapper.Setup(m => m.Map(order)).Returns(orderDto);
        _mockUserRepository.Setup(r => r.GetByIdAsync(order.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(user));
        _mockUserMapper.Setup(m => m.Map(user)).Returns(userSummaryDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(orderDto);
        result.Value.User.Should().Be(userSummaryDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = 1;
        var query = new GetOrderByIdQuery(orderId);

        _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Order not found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenUserDoesNotExist()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { Id = orderId, OrderNumber = "ORD123", UserId = 1 };
        var orderDto = new OrderDto { Id = orderId, OrderNumber = "ORD123", User = new OrderUserDto { Id = 1 } };
        var query = new GetOrderByIdQuery(orderId);

        _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(order));
        _mockOrderMapper.Setup(m => m.Map(order)).Returns(orderDto);
        _mockUserRepository.Setup(r => r.GetByIdAsync(order.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("User not found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }
}

