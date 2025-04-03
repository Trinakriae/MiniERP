using MiniERP.Application.Addresses.Dtos;
using MiniERP.RestApi.Abstractions.AddressBooks;
using MiniERP.RestApi.Models.AddressBooks;

namespace MiniERP.RestApi.Mappers.AddressBooks;

public class UpdateAddressRequestMapper : IAddressRequestMapper<UpdateAddressRequest>
{
    public AddressDto MapToAddressDto(UpdateAddressRequest request) => new()
    {
        Id = request.Id,
        Street = request.Street,
        City = request.City,
        State = request.State,
        PostalCode = request.PostalCode,
        Country = request.Country,
        User = new AddressUserDto { Id = request.UserId }
    };
}
