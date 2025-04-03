using MiniERP.Orders.Domain.Enums;

namespace MiniERP.RestApi.Models.Orders
{
    public record UpdateOrderRequest(
        int Id,
        string OrderNumber,
        OrderStatus Status,
        DateTime Date,
        int UserId,
        DeliveryAddressRequest DeliveryAddress,
        List<OrderLineRequest> Lines);
}
