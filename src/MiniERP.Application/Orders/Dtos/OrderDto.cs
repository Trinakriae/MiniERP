using MiniERP.Orders.Domain.Enums;

namespace MiniERP.Application.Orders.Dtos;

public class OrderDto
{
    public int? Id { get; set; }
    public string OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime Date { get; set; }
    public OrderUserDto User { get; set; }
    public DeliveryAddressDto DeliveryAddress { get; set; }
    public List<OrderLineDto> Lines { get; set; }
}