using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.AddressBook.Infrastructure.Database;

namespace MiniERP.AddressBook.Tests.Infrastructure.Data;

public class AddressBookContextTests : IDisposable
{
    private readonly DbContextOptions<AddressBookContext> _options;

    public AddressBookContextTests()
    {
        _options = new DbContextOptionsBuilder<AddressBookContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddressTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new AddressBookContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(Address));

        // Assert
        entityType.GetTableName().Should().Be(nameof(Address));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey.Properties.Single().Name
            .Should().Be(nameof(Address.Id));

        var streetProperty = entityType.FindProperty(nameof(Address.Street));
        streetProperty.Should().NotBeNull();
        streetProperty.IsNullable
            .Should().BeFalse();

        var cityProperty = entityType.FindProperty(nameof(Address.City));
        cityProperty.Should().NotBeNull();
        cityProperty.IsNullable
            .Should().BeFalse();

        var stateProperty = entityType.FindProperty(nameof(Address.State));
        stateProperty.Should().NotBeNull();
        stateProperty.IsNullable
            .Should().BeFalse();

        var postalCodeProperty = entityType.FindProperty(nameof(Address.PostalCode));
        postalCodeProperty.Should().NotBeNull();
        postalCodeProperty.IsNullable
            .Should().BeFalse();

        var countryProperty = entityType.FindProperty(nameof(Address.Country));
        countryProperty.Should().NotBeNull();
        countryProperty.IsNullable
            .Should().BeFalse();

        var isPrimaryProperty = entityType.FindProperty(nameof(Address.IsPrimary));
        isPrimaryProperty.Should().NotBeNull();
        isPrimaryProperty.IsNullable
            .Should().BeFalse();

        var userIdProperty = entityType.FindProperty(nameof(Address.UserId));
        userIdProperty.Should().NotBeNull();
        userIdProperty.IsNullable
            .Should().BeFalse();
    }

    [Fact]
    public async Task Address_WhenCreated_ShouldSaveCorrectly()
    {
        await using var context = new AddressBookContext(_options);

        // Arrange
        var address = new Address
        {
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "USA",
            IsPrimary = true,
            UserId = 1
        };

        // Act
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();

        // Assert
        var savedAddress = await context.Addresses.FirstOrDefaultAsync(a => a.Id == address.Id);
        savedAddress.Should().NotBeNull();
        savedAddress.Street.Should().Be("123 Main St");
    }

    [Fact]
    public async Task Address_WhenDeleted_ShouldBeRemoved()
    {
        await using var context = new AddressBookContext(_options);

        // Arrange
        var address = new Address
        {
            Street = "123 Main St",
            City = "Anytown",
            State = "Anystate",
            PostalCode = "12345",
            Country = "USA",
            IsPrimary = true,
            UserId = 1
        };
        await context.Addresses.AddAsync(address);
        await context.SaveChangesAsync();

        // Act
        context.Addresses.Remove(address);
        await context.SaveChangesAsync();

        // Assert
        var savedAddress = await context.Addresses.FirstOrDefaultAsync(a => a.Id == address.Id);
        savedAddress.Should().BeNull();
    }

    public void Dispose()
    {
        using var context = new AddressBookContext(_options);
        context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}







