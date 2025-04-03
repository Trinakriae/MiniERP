using FluentAssertions;

using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Addresses.Mappers;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Addresses.Tests.Mappers;

public class UserToAddressUserDtoMapperTests
{
    private readonly UserToAddressUserDtoMapper _mapper;

    public UserToAddressUserDtoMapperTests()
    {
        _mapper = new UserToAddressUserDtoMapper();
    }

    [Fact]
    public void Map_UserToAddressUserDto_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var result = _mapper.Map(user);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
        result.FirstName.Should().Be(user.FirstName);
        result.LastName.Should().Be(user.LastName);
    }

    [Fact]
    public void Map_AddressUserDtoToUser_ShouldMapCorrectly()
    {
        // Arrange
        var userDto = new AddressUserDto
        {
            Id = 1,
            FirstName = "Jane",
            LastName = "Smith"
        };

        // Act
        var result = _mapper.Map(userDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(userDto.Id);
        result.FirstName.Should().Be(userDto.FirstName);
        result.LastName.Should().Be(userDto.LastName);
    }

    [Fact]
    public void Map_NullUser_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((User)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Map_NullAddressUserDto_ShouldReturnNull()
    {
        // Act
        var result = _mapper.Map((AddressUserDto)null);

        // Assert
        result.Should().BeNull();
    }
}
