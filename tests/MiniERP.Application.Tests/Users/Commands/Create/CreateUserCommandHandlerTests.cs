using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Users.Commands.Create;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Users.Commands.Create
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IMapper<User, Application.Users.Dtos.UserDto>> _mockUserMapper;
        private readonly Mock<IValidator<CreateUserCommand>> _mockValidator;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUserMapper = new Mock<IMapper<User, Application.Users.Dtos.UserDto>>();
            _mockValidator = new Mock<IValidator<CreateUserCommand>>();
            _handler = new CreateUserCommandHandler(
                _mockUserRepository.Object,
                _mockUserMapper.Object,
                _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenUserIsValid()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto { Id = 1, FirstName = "Test", LastName = "User", Email = "test@example.com", PhoneNumber = "1234567890" };
            var user = new User { Id = 1, FirstName = "Test", LastName = "User", Email = "test@example.com", PhoneNumber = "1234567890" };
            var command = new CreateUserCommand(userDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockUserMapper.Setup(m => m.Map(userDto)).Returns(user);
            _mockUserRepository.Setup(r => r.AddAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserIsInvalid()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto { Id = 1, FirstName = "Test", LastName = "User", Email = "invalid-email", PhoneNumber = "1234567890" };
            var command = new CreateUserCommand(userDto);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new("UserDto.Email", "Email must be valid.")
                }));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserDtoIsNull()
        {
            // Arrange
            var command = new CreateUserCommand(null);

            _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new("UserDto", "User must not be null.")
                }));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailed.Should().BeTrue();
        }
    }
}

