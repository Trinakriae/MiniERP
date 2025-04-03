using FluentAssertions;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Addresses.Mappers;

namespace MiniERP.Application.Addresses.Tests.Mappers;

public class AddressMapperTests
{
    private readonly AddressMapper _mapper;

    public AddressMapperTests()
    {
        _mapper = new AddressMapper();
    }

    [Fact]
    public void Map_AddressDtoToAddress_ShouldMapCorrectly()
    {
        // Arrange
        var dto = new AddressDto
        {
            Id = 1,
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "Country",
            IsPrimary = true,
            User = new AddressUserDto { Id = 10 }
        };

        // Act
        var result = _mapper.Map(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(dto.Id);
        result.Street.Should().Be(dto.Street);
        result.City.Should().Be(dto.City);
        result.State.Should().Be(dto.State);
        result.PostalCode.Should().Be(dto.PostalCode);
        result.Country.Should().Be(dto.Country);
        result.IsPrimary.Should().Be(dto.IsPrimary);
        result.UserId.Should().Be(dto.User.Id);
    }

    [Fact]
    public void Map_AddressToAddressDto_ShouldMapCorrectly()
    {
        // Arrange
        var entity = new Address
        {
            Id = 1,
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "Country",
            IsPrimary = true,
            UserId = 10
        };

        // Act
        var result = _mapper.Map(entity);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(entity.Id);
        result.Street.Should().Be(entity.Street);
        result.City.Should().Be(entity.City);
        result.State.Should().Be(entity.State);
        result.PostalCode.Should().Be(entity.PostalCode);
        result.Country.Should().Be(entity.Country);
        result.IsPrimary.Should().Be(entity.IsPrimary);
        result.User.Should().BeNull(); // Assuming User is not populated in this mapping
    }

    [Fact]
    public void Map_NullAddressDto_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((AddressDto)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_NullAddress_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((Address)null);

        // Assert
        result.Should().BeNull();
    }
}
