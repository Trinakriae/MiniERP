using MiniERP.Orders.Domain.Enums;

namespace MiniERP.Orders.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Created;
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public DeliveryAddress DeliveryAddress { get; set; }
    public IEnumerable<OrderLine> Lines { get; set; } = new List<OrderLine>();
}
