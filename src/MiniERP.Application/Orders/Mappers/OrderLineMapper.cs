using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Orders.Mappers;

public class OrderLineMapper : IMapper<OrderLine, OrderLineDto>
{
    public OrderLine Map(OrderLineDto orderLineDto)
    {
        if (orderLineDto == null)
        {
            return null;
        }

        return new OrderLine
        {
            Id = orderLineDto.Id ?? default,
            ProductId = orderLineDto.ProductId,
            Quantity = orderLineDto.Quantity,
            Price = orderLineDto.Price
        };
    }

    public OrderLineDto Map(OrderLine orderLine)
    {
        if (orderLine == null)
        {
            return null;
        }

        return new OrderLineDto
        {
            Id = orderLine.Id,
            ProductId = orderLine.ProductId,
            Quantity = orderLine.Quantity,
            Price = orderLine.Price
        };
    }
}


