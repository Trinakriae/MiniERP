using MiniERP.Application.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(int orderId)
        : base($"Order with ID {orderId} was not found.") { }
}
