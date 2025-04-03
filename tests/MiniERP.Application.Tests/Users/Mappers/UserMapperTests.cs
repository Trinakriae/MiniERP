using FluentAssertions;

using MiniERP.Application.Users.Dtos;
using MiniERP.Application.Users.Mappers;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Tests.Mappers
{
    public class UserMapperTests
    {
        private readonly UserMapper _mapper;

        public UserMapperTests()
        {
            _mapper = new UserMapper();
        }

        [Fact]
        public void Map_UserDtoToUser_ShouldMapCorrectly()
        {
            // Arrange
            var dto = new UserDto
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                PhoneNumber = "123-456-7890"
            };

            // Act
            var result = _mapper.Map(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(dto.Id);
            result.FirstName.Should().Be(dto.FirstName);
            result.LastName.Should().Be(dto.LastName);
            result.Email.Should().Be(dto.Email);
            result.PhoneNumber.Should().Be(dto.PhoneNumber);
        }

        [Fact]
        public void Map_UserToUserDto_ShouldMapCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Smith",
                Email = "bob.smith@example.com",
                PhoneNumber = "098-765-4321"
            };

            // Act
            var result = _mapper.Map(user);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);
            result.Email.Should().Be(user.Email);
            result.PhoneNumber.Should().Be(user.PhoneNumber);
        }

        [Fact]
        public void Map_NullUserDto_ShouldReturnNull()
        {
            // Act
            var result = _mapper.Map((UserDto)null);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Map_NullUser_ShouldReturnNull()
        {
            // Act
            var result = _mapper.Map((User)null);

            // Assert
            result.Should().BeNull();
        }
    }
}
