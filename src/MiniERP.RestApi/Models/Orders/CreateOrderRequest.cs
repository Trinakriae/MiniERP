namespace MiniERP.RestApi.Models.Orders;

public record CreateOrderRequest(
    string OrderNumber,
    int Status,
    DateTime Date,
    int UserId,
    DeliveryAddressRequest DeliveryAddress,
    List<OrderLineRequest> Lines);
