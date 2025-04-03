namespace MiniERP.RestApi.Models.AddressBooks;

public record UpdateAddressRequest(
    int Id,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    int UserId);
