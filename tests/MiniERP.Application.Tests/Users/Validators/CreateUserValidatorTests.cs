using FluentValidation.TestHelper;

using MiniERP.Application.Users.Commands.Create;
using MiniERP.Application.Users.Validators;

namespace MiniERP.Application.Tests.Users.Validators
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserCommandValidator _validator;

        public CreateUserValidatorTests()
        {
            _validator = new CreateUserCommandValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenUserIsNull()
        {
            // Arrange
            CreateUserCommand? command = new(null as Application.Users.Dtos.UserDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserDto).WithErrorMessage("User must not be null.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenFirstNameIsEmpty()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto
            {
                Id = 1,
                FirstName = string.Empty,
                LastName = "User",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };
            var command = new CreateUserCommand(userDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserDto.FirstName).WithErrorMessage("First name must not be empty.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenLastNameIsEmpty()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = string.Empty,
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };
            var command = new CreateUserCommand(userDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserDto.LastName).WithErrorMessage("Last name must not be empty.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenEmailIsEmpty()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = string.Empty,
                PhoneNumber = "1234567890"
            };
            var command = new CreateUserCommand(userDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserDto.Email).WithErrorMessage("Email must not be empty.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenEmailIsInvalid()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "invalid-email",
                PhoneNumber = "1234567890"
            };
            var command = new CreateUserCommand(userDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserDto.Email).WithErrorMessage("Email must be valid.");
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenPhoneNumberIsEmpty()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PhoneNumber = string.Empty
            };
            var command = new CreateUserCommand(userDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserDto.PhoneNumber).WithErrorMessage("Phone number must not be empty.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenUserIsValid()
        {
            // Arrange
            var userDto = new Application.Users.Dtos.UserDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                PhoneNumber = "1234567890"
            };
            var command = new CreateUserCommand(userDto);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}


