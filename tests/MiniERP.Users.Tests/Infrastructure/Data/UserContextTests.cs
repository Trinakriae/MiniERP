using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Users.Domain.Entities;
using MiniERP.Users.Infrastructure.Database;

namespace MiniERP.Users.Tests.Infrastructure.Data;

public class UserContextTests : IDisposable
{
    private readonly DbContextOptions<UserContext> _options;

    public UserContextTests()
    {
        _options = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task UserTable_WhenConfigured_HasCorrectConfiguration()
    {
        await using var context = new UserContext(_options);

        // Act
        var entityType = context.Model.FindEntityType(typeof(User));

        // Assert
        entityType.GetTableName().Should().Be(nameof(User));

        var primaryKey = entityType.FindPrimaryKey();
        primaryKey.Should().NotBeNull();
        primaryKey.Properties.Single().Name
            .Should().Be(nameof(User.Id));

        var firstNameProperty = entityType.FindProperty(nameof(User.FirstName));
        firstNameProperty.Should().NotBeNull();
        firstNameProperty.IsNullable
            .Should().BeFalse();

        var lastNameProperty = entityType.FindProperty(nameof(User.LastName));
        lastNameProperty.Should().NotBeNull();
        lastNameProperty.IsNullable
            .Should().BeFalse();

        var emailProperty = entityType.FindProperty(nameof(User.Email));
        emailProperty.Should().NotBeNull();
        emailProperty.IsNullable
            .Should().BeFalse();

        var phoneNumberProperty = entityType.FindProperty(nameof(User.PhoneNumber));
        phoneNumberProperty.Should().NotBeNull();
        phoneNumberProperty.IsNullable
            .Should().BeFalse();
    }

    [Fact]
    public async Task User_WhenCreated_ShouldSaveCorrectly()
    {
        await using var context = new UserContext(_options);

        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890"
        };

        // Act
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        savedUser.Should().NotBeNull();
        savedUser.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task User_WhenDeleted_ShouldBeRemoved()
    {
        await using var context = new UserContext(_options);

        // Arrange
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "123-456-7890"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        // Act
        context.Users.Remove(user);
        await context.SaveChangesAsync();

        // Assert
        var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        savedUser.Should().BeNull();
    }

    public void Dispose()
    {
        using var context = new UserContext(_options);
        context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }
}








