using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Enums;
using MiniERP.RestApi.Abstractions.Orders;
using MiniERP.RestApi.Models.Orders;

namespace MiniERP.RestApi.Mappers.Orders;

public class CreateOrderRequestMapper : IOrderRequestMapper<CreateOrderRequest>
{
    public OrderDto MapToOrderDto(CreateOrderRequest request) => new()
    {
        OrderNumber = request.OrderNumber,
        Status = (OrderStatus)request.Status,
        Date = request.Date,
        User = new OrderUserDto { Id = request.UserId },
        DeliveryAddress = new DeliveryAddressDto
        {
            Id = request.DeliveryAddress.Id,
            Street = request.DeliveryAddress.Street,
            City = request.DeliveryAddress.City,
            State = request.DeliveryAddress.State,
            PostalCode = request.DeliveryAddress.PostalCode,
            Country = request.DeliveryAddress.Country
        },
        Lines = [.. request.Lines.Select(line => new OrderLineDto
        {
            ProductId = line.ProductId,
            Quantity = line.Quantity,
            Price = line.Price
        })]
    };
}