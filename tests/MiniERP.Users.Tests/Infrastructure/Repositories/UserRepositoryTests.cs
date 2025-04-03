using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using MiniERP.Users.Domain.Entities;
using MiniERP.Users.Infrastructure.Database;
using MiniERP.Users.Infrastructure.Repositories;
using MiniERP.Users.Tests.Mocks;

using Moq;
namespace MiniERP.Users.Tests.Infrastructure.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<UserContext> _mockContext;
    private Mock<DbSet<User>> _mockDbSet;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _mockContext = new Mock<UserContext>(new DbContextOptions<UserContext>());
        _mockDbSet = new Mock<DbSet<User>>();

        _mockContext.Setup(m => m.Users).Returns(_mockDbSet.Object);
        _repository = new UserRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        var data = new List<User> { user }.AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<User>(data);

        _mockContext.Setup(m => m.Users).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(user);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFail_WhenUserDoesNotExist()
    {
        // Arrange
        var data = new List<User>().AsQueryable();
        _mockDbSet = MockAsyncQueryCollection.GetMockDbSet<User>(data);

        _mockContext.Setup(m => m.Users).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == $"User with ID {1} was not found");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };
        var data = users.AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<User>(data);

        _mockContext.Setup(m => m.Users).Returns(mockSet.Object);

        // Act
        var result = await _repository.GetAsync(CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnSuccess_WhenUserIsAdded()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        // Act
        var result = await _repository.AddAsync(user, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockDbSet.Verify(m => m.Add(user), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenUserIsUpdated()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
        _mockContext.Setup(m => m.Users).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.UpdateAsync(user, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Update(user), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenUserIsDeleted()
    {
        // Arrange
        var user = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

        _mockContext.Setup(m => m.Users.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _repository.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockContext.Verify(m => m.Users.Remove(user), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFail_WhenUserDoesNotExist()
    {
        // Arrange
        var data = new List<User>().AsQueryable();
        var mockSet = MockAsyncQueryCollection.GetMockDbSet<User>(data);

        _mockContext.Setup(m => m.Users.FindAsync(It.IsAny<int>()))
             .ReturnsAsync(null as User);

        // Act
        var result = await _repository.DeleteAsync(999, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "User not found");
    }
}

