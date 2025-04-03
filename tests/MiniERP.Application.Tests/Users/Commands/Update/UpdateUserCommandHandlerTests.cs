using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Users.Commands.Update;
using MiniERP.Application.Users.Dtos;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Users.Commands.Update
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IMapper<User, Application.Users.Dtos.UserDto>> _mockUserMapper;
        private readonly Mock<IValidator<UpdateUserCommand>> _mockValidator;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserMapper = new Mock<IMapper<User, Application.Users.Dtos.UserDto>>();
            _mockValidator = new Mock<IValidator<UpdateUserCommand>>();
            _handler = new UpdateUserCommandHandler(
                _mockUserRepository.Object,
                _mockUserMapper.Object,
                _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenUserIsValid()
        {
            // Arrange
            var userDto = new UserDto { Id = 1, FirstName = "Test", LastName = "User", Email = "test@example.com", PhoneNumber = "1234567890" };
            var user = new User { Id = 1, FirstName = "Test", LastName = "User", Email = "test@example.com", PhoneNumber = "1234567890" };
            var command = new UpdateUserCommand(userDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockUserMapper.Setup(m => m.Map(userDto)).Returns(user);
            _mockUserRepository.Setup(r => r.UpdateAsync(user, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok());
            _mockUserRepository.Setup(r => r.GetByIdAsync(userDto.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(user));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserIsInvalid()
        {
            // Arrange
            var userDto = new UserDto { Id = 1, FirstName = "Test", LastName = "User", Email = "invalid-email", PhoneNumber = "1234567890" };
            var command = new UpdateUserCommand(userDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure("UserDto.Email", "Email must be valid.")
                ]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserDtoIsNull()
        {
            // Arrange
            var command = new UpdateUserCommand(null);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(
                [
                    new ValidationFailure("UserDto", "User must not be null.")
                ]));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserIsNotFound()
        {
            // Arrange
            var userDto = new UserDto { Id = 1 };
            var command = new UpdateUserCommand(userDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockUserRepository.Setup(r => r.GetByIdAsync(userDto.Id.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail(ResultErrors.NotFound<User>(userDto.Id.Value)));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UserNotFoundException>();
        }
    }
}

