namespace MiniERP.RestApi.Models.Orders;

public record OrderLineRequest(
    int? Id,
    int ProductId,
    int Quantity,
    decimal Price);