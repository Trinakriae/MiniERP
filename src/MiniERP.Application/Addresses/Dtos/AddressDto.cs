namespace MiniERP.Application.Addresses.Dtos;

public class AddressDto
{
    public int? Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public bool IsPrimary { get; set; }
    public AddressUserDto User { get; set; }
}





