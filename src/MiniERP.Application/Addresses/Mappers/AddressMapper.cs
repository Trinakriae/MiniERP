using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.Application.Addresses.Mappers;

public class AddressMapper : IMapper<Address, AddressDto>
{

    public AddressMapper()
    {
    }

    public Address Map(AddressDto dto)
    {
        if (dto == null)
        {
            return null;
        }

        return new Address
        {
            Id = dto.Id ?? default,
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            IsPrimary = dto.IsPrimary,
            UserId = dto.User.Id,
        };
    }

    public AddressDto Map(Address entity)
    {
        if (entity == null)
        {
            return null;
        }
        return new AddressDto
        {
            Id = entity.Id,
            Street = entity.Street,
            City = entity.City,
            State = entity.State,
            PostalCode = entity.PostalCode,
            Country = entity.Country,
            IsPrimary = entity.IsPrimary,
        };
    }
}
