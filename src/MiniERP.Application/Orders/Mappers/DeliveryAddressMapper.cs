using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Orders.Mappers;

public class DeliveryAddressMapper : IMapper<DeliveryAddress, DeliveryAddressDto>
{
    public DeliveryAddress Map(DeliveryAddressDto deliveryAddressDto)
    {
        if (deliveryAddressDto == null)
        {
            return null;
        }

        return new DeliveryAddress
        {
            Id = deliveryAddressDto.Id ?? default,
            Street = deliveryAddressDto.Street,
            City = deliveryAddressDto.City,
            State = deliveryAddressDto.State,
            PostalCode = deliveryAddressDto.PostalCode,
            Country = deliveryAddressDto.Country
        };
    }

    public DeliveryAddressDto Map(DeliveryAddress deliveryAddress)
    {
        if (deliveryAddress == null)
        {
            return null;
        }

        return new DeliveryAddressDto
        {
            Id = deliveryAddress.Id,
            Street = deliveryAddress.Street,
            City = deliveryAddress.City,
            State = deliveryAddress.State,
            PostalCode = deliveryAddress.PostalCode,
            Country = deliveryAddress.Country
        };
    }
}


