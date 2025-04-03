namespace MiniERP.RestApi.Models.AddressBooks;

public record CreateAddressRequest(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    int UserId);
