using FluentAssertions;

using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Queries.Get;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Queries.Get;

public class GetOrdersQueryHandlerTests
{
    private readonly Mock<IRepository<Order>> _mockOrderRepository;
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper<Order, OrderDto>> _mockOrderMapper;
    private readonly Mock<IMapper<User, OrderUserDto>> _mockUserMapper;
    private readonly GetOrdersQueryHandler _handler;

    public GetOrdersQueryHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order>>();
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockOrderMapper = new Mock<IMapper<Order, OrderDto>>();
        _mockUserMapper = new Mock<IMapper<User, OrderUserDto>>();
        _handler = new GetOrdersQueryHandler(
            _mockOrderRepository.Object,
            _mockUserRepository.Object,
            _mockOrderMapper.Object,
            _mockUserMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOrders_WhenOrdersExist()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { Id = 1, OrderNumber = "ORD123", UserId = 1 },
            new() { Id = 2, OrderNumber = "ORD124", UserId = 2 }
        };
        var orderDtos = new List<OrderDto>
        {
            new() { Id = 1, OrderNumber = "ORD123", User = new OrderUserDto { Id = 1 } },
            new() { Id = 2, OrderNumber = "ORD124", User = new OrderUserDto { Id = 2 } }
        };
        var users = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };
        var userSummaryDtos = new List<OrderUserDto>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };
        var query = new GetOrdersQuery();

        _mockOrderRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(orders.AsEnumerable()));
        _mockOrderMapper.Setup(m => m.Map(It.IsAny<Order>())).Returns((Order order) => orderDtos.First(dto => dto.Id == order.Id));
        _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int id, CancellationToken token) => Result.Ok(users.First(user => user.Id == id)));
        _mockUserMapper.Setup(m => m.Map(It.IsAny<User>())).Returns((User user) => userSummaryDtos.First(dto => dto.Id == user.Id));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(orderDtos, options => options.Excluding(dto => dto.User));
        foreach (var orderDto in result.Value)
        {
            var expectedUser = userSummaryDtos.First(dto => dto.Id == orderDto.User.Id);
            orderDto.User.Should().Be(expectedUser);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenNoOrdersExist()
    {
        // Arrange
        var query = new GetOrdersQuery();

        _mockOrderRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("No orders found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenUserDoesNotExist()
    {
        // Arrange
        var orders = new List<Order>
        {
            new() { Id = 1, OrderNumber = "ORD123", UserId = 1 },
            new() { Id = 2, OrderNumber = "ORD124", UserId = 2 }
        };
        var orderDtos = new List<OrderDto>
        {
            new() { Id = 1, OrderNumber = "ORD123", User = new OrderUserDto { Id = 1 } },
            new() { Id = 2, OrderNumber = "ORD124", User = new OrderUserDto { Id = 2 } }
        };
        var query = new GetOrdersQuery();

        _mockOrderRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(orders.AsEnumerable()));
        _mockOrderMapper.Setup(m => m.Map(It.IsAny<Order>())).Returns((Order order) => orderDtos.First(dto => dto.Id == order.Id));
        _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("User not found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }
}

