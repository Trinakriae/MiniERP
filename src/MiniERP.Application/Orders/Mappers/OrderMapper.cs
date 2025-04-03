using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Orders.Mappers;

public class OrderMapper(
    IMapper<OrderLine, OrderLineDto> orderLineMapper,
    IMapper<DeliveryAddress, DeliveryAddressDto> deliveryAddressMapper) : IMapper<Order, OrderDto>
{
    private readonly IMapper<OrderLine, OrderLineDto> _orderLineMapper = orderLineMapper;
    private readonly IMapper<DeliveryAddress, DeliveryAddressDto> _deliveryAddressMapper = deliveryAddressMapper;

    public Order Map(OrderDto orderDto)
    {
        if (orderDto == null)
        {
            return null;
        }

        return new Order
        {
            Id = orderDto.Id ?? default,
            OrderNumber = orderDto.OrderNumber,
            Status = orderDto.Status,
            Date = orderDto.Date,
            UserId = orderDto.User.Id,
            DeliveryAddress = _deliveryAddressMapper.Map(orderDto.DeliveryAddress),
            Lines = [.. orderDto.Lines.Select(_orderLineMapper.Map)],
        };
    }

    public OrderDto Map(Order order)
    {
        if (order == null)
        {
            return null;
        }

        return new OrderDto
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            Date = order.Date,
            User = new OrderUserDto { Id = order.UserId },
            DeliveryAddress = _deliveryAddressMapper.Map(order.DeliveryAddress),
            Lines = [.. order.Lines.Select(_orderLineMapper.Map)],
        };
    }
}

