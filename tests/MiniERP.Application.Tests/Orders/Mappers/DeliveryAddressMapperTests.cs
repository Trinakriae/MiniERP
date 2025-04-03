using FluentAssertions;

using MiniERP.Application.Orders.Dtos;
using MiniERP.Application.Orders.Mappers;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Tests.Orders.Mappers;

public class DeliveryAddressMapperTests
{
    private readonly DeliveryAddressMapper _mapper;

    public DeliveryAddressMapperTests()
    {
        _mapper = new DeliveryAddressMapper();
    }

    [Fact]
    public void Map_WhenDeliveryAddressDtoIsNull_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((DeliveryAddressDto)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_WhenDeliveryAddressIsNull_ShouldReturnNull()
    {
        // Act
        // Act
        var result = _mapper.Map((DeliveryAddress)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_ShouldMapDeliveryAddressDtoToDeliveryAddress()
    {
        // Arrange
        var deliveryAddressDto = new DeliveryAddressDto
        {
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "USA"
        };

        // Act
        var deliveryAddress = _mapper.Map(deliveryAddressDto);

        // Assert
        deliveryAddress.Should().NotBeNull();
        deliveryAddress.Street.Should().Be(deliveryAddressDto.Street);
        deliveryAddress.City.Should().Be(deliveryAddressDto.City);
        deliveryAddress.State.Should().Be(deliveryAddressDto.State);
        deliveryAddress.PostalCode.Should().Be(deliveryAddressDto.PostalCode);
        deliveryAddress.Country.Should().Be(deliveryAddressDto.Country);
    }

    [Fact]
    public void Map_ShouldMapDeliveryAddressToDeliveryAddressDto()
    {
        // Arrange
        var deliveryAddress = new DeliveryAddress
        {
            Id = 1,
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "USA"
        };

        // Act
        var deliveryAddressDto = _mapper.Map(deliveryAddress);

        // Assert
        deliveryAddressDto.Should().NotBeNull();
        deliveryAddressDto.Street.Should().Be(deliveryAddress.Street);
        deliveryAddressDto.City.Should().Be(deliveryAddress.City);
        deliveryAddressDto.State.Should().Be(deliveryAddress.State);
        deliveryAddressDto.PostalCode.Should().Be(deliveryAddress.PostalCode);
        deliveryAddressDto.Country.Should().Be(deliveryAddress.Country);
    }
}


