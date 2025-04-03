namespace MiniERP.RestApi.Models.Orders;

public record DeliveryAddressRequest(
    int? Id,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country);