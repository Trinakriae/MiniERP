using FluentAssertions;

using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Mappers;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Orders.Domain.Enums;

namespace MiniERP.Application.Tests.Orders.Mappers;

public class OrderMapperTests
{
    private readonly OrderMapper _mapper;

    public OrderMapperTests()
    {
        var orderLineMapper = new OrderLineMapper();
        var deliveryAddressMapper = new DeliveryAddressMapper();
        _mapper = new OrderMapper(orderLineMapper, deliveryAddressMapper);
    }

    [Fact]
    public void Map_WhenOrderDtoIsNull_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((OrderDto)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_WhenOrderIsNull_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((Order)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_ShouldMapOrderDtoToOrde()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            Id = 1,
            OrderNumber = "ORD123",
            Status = OrderStatus.Created,
            Date = DateTime.UtcNow,
            User = new OrderUserDto { Id = 1 },
            DeliveryAddress = new DeliveryAddressDto
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "Anystate",
                PostalCode = "12345",
                Country = "USA"
            },
            Lines =
            [
                new() { Id = 1, ProductId = 1, Quantity = 2, Price = 10.0m },
                new() { Id = 2, ProductId = 2, Quantity = 1, Price = 20.0m }
            ]
        };

        // Act
        var order = _mapper.Map(orderDto);

        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(orderDto.Id);
        order.OrderNumber.Should().Be(orderDto.OrderNumber);
        order.Status.Should().Be(orderDto.Status);
        order.Date.Should().Be(orderDto.Date);
        order.UserId.Should().Be(orderDto.User.Id);
        order.DeliveryAddress.Should().NotBeNull();
        order.DeliveryAddress.Street.Should().Be(orderDto.DeliveryAddress.Street);
        order.DeliveryAddress.City.Should().Be(orderDto.DeliveryAddress.City);
        order.DeliveryAddress.State.Should().Be(orderDto.DeliveryAddress.State);
        order.DeliveryAddress.PostalCode.Should().Be(orderDto.DeliveryAddress.PostalCode);
        order.DeliveryAddress.Country.Should().Be(orderDto.DeliveryAddress.Country);
        order.Lines.Should().HaveCount(2);
        order.Lines.First().Id.Should().Be(orderDto.Lines.First().Id);
        order.Lines.First().ProductId.Should().Be(orderDto.Lines.First().ProductId);
        order.Lines.First().Quantity.Should().Be(orderDto.Lines.First().Quantity);
        order.Lines.First().Price.Should().Be(orderDto.Lines.First().Price);
    }

    [Fact]
    public void Map_ShouldMapOrderDtoToOrder_WithoutId()
    {
        // Arrange
        var orderDto = new OrderDto
        {
            OrderNumber = "ORD123",
            Status = OrderStatus.Created,
            Date = DateTime.UtcNow,
            User = new OrderUserDto { Id = 1 },
            DeliveryAddress = new DeliveryAddressDto
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "Anystate",
                PostalCode = "12345",
                Country = "USA"
            },
            Lines =
            [
                new() { Id = 1, ProductId = 1, Quantity = 2, Price = 10.0m },
                new() { Id = 2, ProductId = 2, Quantity = 1, Price = 20.0m }
            ]
        };

        // Act
        var order = _mapper.Map(orderDto);

        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(default);
        order.OrderNumber.Should().Be(orderDto.OrderNumber);
        order.Status.Should().Be(orderDto.Status);
        order.Date.Should().Be(orderDto.Date);
        order.UserId.Should().Be(orderDto.User.Id);
        order.DeliveryAddress.Should().NotBeNull();
        order.DeliveryAddress.Street.Should().Be(orderDto.DeliveryAddress.Street);
        order.DeliveryAddress.City.Should().Be(orderDto.DeliveryAddress.City);
        order.DeliveryAddress.State.Should().Be(orderDto.DeliveryAddress.State);
        order.DeliveryAddress.PostalCode.Should().Be(orderDto.DeliveryAddress.PostalCode);
        order.DeliveryAddress.Country.Should().Be(orderDto.DeliveryAddress.Country);
        order.Lines.Should().HaveCount(2);
        order.Lines.First().Id.Should().Be(orderDto.Lines.First().Id);
        order.Lines.First().ProductId.Should().Be(orderDto.Lines.First().ProductId);
        order.Lines.First().Quantity.Should().Be(orderDto.Lines.First().Quantity);
        order.Lines.First().Price.Should().Be(orderDto.Lines.First().Price);
    }

    [Fact]
    public void Map_ShouldMapOrderToOrderDto()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            OrderNumber = "ORD123",
            Status = OrderStatus.Created,
            Date = DateTime.UtcNow,
            UserId = 1,
            DeliveryAddress = new DeliveryAddress
            {
                Id = 1,
                Street = "123 Main St",
                City = "Anytown",
                State = "Anystate",
                PostalCode = "12345",
                Country = "USA"
            },
            Lines =
            [
                new() { Id = 1, ProductId = 1, Quantity = 2, Price = 10.0m },
                new() { Id = 2, ProductId = 2, Quantity = 1, Price = 20.0m }
            ]
        };

        // Act
        var orderDto = _mapper.Map(order);

        // Assert
        orderDto.Should().NotBeNull();
        orderDto.Id.Should().Be(order.Id);
        orderDto.OrderNumber.Should().Be(order.OrderNumber);
        orderDto.Status.Should().Be(order.Status);
        orderDto.Date.Should().Be(order.Date);
        orderDto.User.Id.Should().Be(order.UserId);
        orderDto.DeliveryAddress.Should().NotBeNull();
        orderDto.DeliveryAddress.Street.Should().Be(order.DeliveryAddress.Street);
        orderDto.DeliveryAddress.City.Should().Be(order.DeliveryAddress.City);
        orderDto.DeliveryAddress.State.Should().Be(order.DeliveryAddress.State);
        orderDto.DeliveryAddress.PostalCode.Should().Be(order.DeliveryAddress.PostalCode);
        orderDto.DeliveryAddress.Country.Should().Be(order.DeliveryAddress.Country);
        orderDto.Lines.Should().HaveCount(2);
        orderDto.Lines.First().Id.Should().Be(order.Lines.First().Id);
        orderDto.Lines.First().ProductId.Should().Be(order.Lines.First().ProductId);
        orderDto.Lines.First().Quantity.Should().Be(order.Lines.First().Quantity);
        orderDto.Lines.First().Price.Should().Be(order.Lines.First().Price);
    }
}


