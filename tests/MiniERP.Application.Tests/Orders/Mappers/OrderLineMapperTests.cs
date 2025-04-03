using FluentAssertions;

using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Mappers;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Tests.Orders.Mappers;

public class OrderLineMapperTests
{
    private readonly OrderLineMapper _mapper;

    public OrderLineMapperTests()
    {
        _mapper = new OrderLineMapper();
    }

    [Fact]
    public void Map_WhenOrderLineDtoIsNull_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((OrderLineDto)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_WhenOrderLineIsNull_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((OrderLine)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_ShouldMapOrderLineDtoToOrderLine()
    {
        // Arrange
        var orderLineDto = new OrderLineDto
        {
            Id = 1,
            ProductId = 2,
            Quantity = 3,
            Price = 4.5m
        };

        // Act
        var orderLine = _mapper.Map(orderLineDto);

        // Assert
        orderLine.Should().NotBeNull();
        orderLine.Id.Should().Be(orderLineDto.Id);
        orderLine.ProductId.Should().Be(orderLineDto.ProductId);
        orderLine.Quantity.Should().Be(orderLineDto.Quantity);
        orderLine.Price.Should().Be(orderLineDto.Price);
    }

    [Fact]
    public void Map_ShouldMapOrderLineDtoToOrderLine_WithoutId()
    {
        // Arrange
        var orderLineDto = new OrderLineDto
        {
            ProductId = 2,
            Quantity = 3,
            Price = 4.5m
        };

        // Act
        var orderLine = _mapper.Map(orderLineDto);

        // Assert
        orderLine.Should().NotBeNull();
        orderLine.Id.Should().Be(default);
        orderLine.ProductId.Should().Be(orderLineDto.ProductId);
        orderLine.Quantity.Should().Be(orderLineDto.Quantity);
        orderLine.Price.Should().Be(orderLineDto.Price);
    }

    [Fact]
    public void Map_ShouldMapOrderLineToOrderLineDto()
    {
        // Arrange
        var orderLine = new OrderLine
        {
            Id = 1,
            ProductId = 2,
            Quantity = 3,
            Price = 4.5m
        };

        // Act
        var orderLineDto = _mapper.Map(orderLine);

        // Assert
        orderLineDto.Should().NotBeNull();
        orderLineDto.Id.Should().Be(orderLine.Id);
        orderLineDto.ProductId.Should().Be(orderLine.ProductId);
        orderLineDto.Quantity.Should().Be(orderLine.Quantity);
        orderLineDto.Price.Should().Be(orderLine.Price);
    }
}


