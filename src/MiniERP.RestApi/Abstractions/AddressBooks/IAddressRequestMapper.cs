using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.RestApi.Abstractions.AddressBooks;

public interface IAddressRequestMapper<TRequest>
{
    AddressDto MapToAddressDto(TRequest request);
}
