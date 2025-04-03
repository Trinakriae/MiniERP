using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.AddressBook.Infrastructure.Database;
using MiniERP.AddressBook.Infrastructure.Repositories;
using MiniERP.AddressBook.Tests.Mocks;

using Moq;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiniERP.AddressBook.Tests.Infrastructure.Repositories;

public class AddressRepositoryTests
{
    private readonly Mock<AddressBookContext> _mockContext;
    private Mock<DbSet<Address>> _mockDbSet;
    private readonly AddressRepository _repository;

    public AddressRepositoryTests()
    {
        _mockContext = new Mock<AddressBookContext>(new DbContextOptions<AddressBookContext>());
        _mockDbSet = new Mock<DbSet<Address>>();

        _mockContext.Setup(m => m.Addresses).Returns(_mockDbSet.Object);
        _repository = new AddressRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAddress_WhenAddressExists()
    {
        // Arrange
        var address = new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, UserId = 1 };
        var data = new List<Address> { address }.AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Address>(data);
        ;
        _mockContext.Setup(m => m.Addresses).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(address);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFail_WhenAddressDoesNotExist()
    {
        // Arrange
        var data = new List<Address>().AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<Address>(data);

        _mockContext.Setup(m => m.Addresses).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == $"Address with ID {1} was not found");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAddresses()
    {
        // Arrange
        var addresses = new List<Address>
        {
            new() { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, UserId = 1 },
            new() { Id = 2, Street = "456 Elm St", City = "Othertown", State = "Otherstate", PostalCode = "67890", Country = "USA", IsPrimary = false, UserId = 2 }
        };
        var data = addresses.AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Address>(data);

        _mockContext.Setup(m => m.Addresses).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(addresses);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnSuccess_WhenAddressIsAdded()
    {
        // Arrange
        var address = new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, UserId = 1 };

        // Act
        var result = await _repository.AddAsync(address, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockDbSet.Verify(m => m.Add(address), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenAddressIsUpdated()
    {
        // Arrange
        var address = new Address { Id = 1, Street = "123 Main St", City = "Anytown", State = "Anystate", PostalCode = "12345", Country = "USA", IsPrimary = true, UserId = 1 };
        _mockContext.Setup(m => m.Addresses).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.UpdateAsync(address, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Update(address), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenAddressIsDeleted()
    {
        // Arrange
        var address = new Address
        {
            Id = 1,
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "USA",
            IsPrimary = true,
            UserId = 1
        };

        _mockContext.Setup(m => m.Addresses.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(address);

        // Act
        var result = await _repository.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Addresses.Remove(address), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFail_WhenAddressDoesNotExist()
    {
        // Arrange
        var data = new List<Address>().AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<Address>(data);

        _mockContext.Setup(m => m.Addresses.FindAsync(It.IsAny<int>()))
             .ReturnsAsync(null as Address);

        // Act
        var result = await _repository.DeleteAsync(999, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "Address not found");
    }
}

