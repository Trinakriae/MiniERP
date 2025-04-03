using MiniERP.Application.Orders.Dtos;
using MiniERP.RestApi.Abstractions.Orders;
using MiniERP.RestApi.Models.Orders;

namespace MiniERP.RestApi.Mappers.Orders;

public class UpdateOrderRequestMapper : IOrderRequestMapper<UpdateOrderRequest>
{
    public OrderDto MapToOrderDto(UpdateOrderRequest request) => new()
    {
        Id = request.Id,
        OrderNumber = request.OrderNumber,
        Status = request.Status,
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
            Id = line.Id,
            ProductId = line.ProductId,
            Quantity = line.Quantity,
            Price = line.Price
        })]
    };
}