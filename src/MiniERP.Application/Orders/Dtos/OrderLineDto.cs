namespace MiniERP.Application.Orders.Dtos;

public class OrderLineDto
{
    public int? Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

