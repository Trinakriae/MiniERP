using FluentAssertions;

using FluentResults;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Addresses.Queries.Get;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.AddressBooks.Queries.Get
{
    public class GetAddressesQueryHandlerTests
    {
        private readonly Mock<IRepository<Address>> _mockAddressRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IMapper<Address, AddressDto>> _mockAddressMapper;
        private readonly Mock<IMapper<User, AddressUserDto>> _mockUserMapper;
        private readonly GetAddressesQueryHandler _handler;

        public GetAddressesQueryHandlerTests()
        {
            _mockAddressRepository = new Mock<IRepository<Address>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockAddressMapper = new Mock<IMapper<Address, AddressDto>>();
            _mockUserMapper = new Mock<IMapper<User, AddressUserDto>>();
            _handler = new GetAddressesQueryHandler(
                _mockAddressRepository.Object,
                _mockUserRepository.Object,
                _mockAddressMapper.Object,
                _mockUserMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAddresses_WhenAddressesExist()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, UserId = 1 },
                new() { Id = 2, Street = "456 Elm St", City = "Othertown", State = "Otherstate", PostalCode = "67890", Country = "USA", IsPrimary = false, UserId = 2 }
            };
            var users = new List<User>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };
            var addressDtos = new List<AddressDto>
            {
                new() { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, User = new AddressUserDto { Id = 1, FirstName = "John", LastName = "Doe" } },
                new() { Id = 2, Street = "456 Elm St", City = "Othertown", State = "Otherstate", PostalCode = "67890", Country = "USA", IsPrimary = false, User = new AddressUserDto { Id = 2, FirstName = "Jane", LastName = "Doe" } }
            };
            var query = new GetAddressesQuery();

            _mockAddressRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(addresses.AsEnumerable()));
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int id, CancellationToken ct) => Result.Ok(users.First(u => u.Id == id)));
            _mockAddressMapper.Setup(m => m.Map(It.IsAny<Address>())).Returns((Address address) => addressDtos.First(dto => dto.Id == address.Id));
            _mockUserMapper.Setup(m => m.Map(It.IsAny<User>())).Returns((User user) => addressDtos.First(dto => dto.User.Id == user.Id).User);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(addressDtos);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenNoAddressesExist()
        {
            // Arrange
            var query = new GetAddressesQuery();

            _mockAddressRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail("No addresses found"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserDoesNotExist()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, UserId = 1 }
            };
            var query = new GetAddressesQuery();

            _mockAddressRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(addresses.AsEnumerable()));
            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail("User not found"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}




