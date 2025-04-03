using FluentAssertions;

using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Users.Dtos;
using MiniERP.Application.Users.Queries.GetById;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Users.Queries.GetById
{
    public class GetUserByIdQueryHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IMapper<User, UserDto>> _mockUserMapper;
        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserMapper = new Mock<IMapper<User, UserDto>>();
            _handler = new GetUserByIdQueryHandler(
                _mockUserRepository.Object,
                _mockUserMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var userDto = new UserDto { Id = userId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var query = new GetUserByIdQuery(userId);

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(user));
            _mockUserMapper.Setup(m => m.Map(user)).Returns(userDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(userDto);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var query = new GetUserByIdQuery(userId);

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail("User not found"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}



