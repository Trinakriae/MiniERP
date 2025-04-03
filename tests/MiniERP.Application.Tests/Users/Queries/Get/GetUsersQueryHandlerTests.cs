using FluentAssertions;

using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Users.Dtos;
using MiniERP.Application.Users.Queries.Get;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Users.Queries.Get;

public class GetUsersQueryHandlerTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IMapper<User, UserDto>> _mockUserMapper;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockUserMapper = new Mock<IMapper<User, UserDto>>();
        _handler = new GetUsersQueryHandler(
            _mockUserRepository.Object,
            _mockUserMapper.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };
        var userDtos = new List<UserDto>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };
        var query = new GetUsersQuery();

        _mockUserRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(users.AsEnumerable()));
        _mockUserMapper.Setup(m => m.Map(It.IsAny<User>())).Returns((User user) => userDtos.First(dto => dto.Id == user.Id));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userDtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenNoUsersExist()
    {
        // Arrange
        var query = new GetUsersQuery();

        _mockUserRepository.Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("No users found"));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }
}



