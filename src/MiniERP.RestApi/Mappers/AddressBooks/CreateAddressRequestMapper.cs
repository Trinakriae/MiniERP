using MiniERP.Application.Addresses.Dtos;
using MiniERP.RestApi.Abstractions.AddressBooks;
using MiniERP.RestApi.Models.AddressBooks;

namespace MiniERP.RestApi.Mappers.AddressBooks;

public class CreateAddressRequestMapper : IAddressRequestMapper<CreateAddressRequest>
{
    public AddressDto MapToAddressDto(CreateAddressRequest request) => new()
    {
        Street = request.Street,
        City = request.City,
        State = request.State,
        PostalCode = request.PostalCode,
        Country = request.Country,
        User = new AddressUserDto { Id = request.UserId }
    };
}
