using FluentValidation.TestHelper;

using MiniERP.Application.Users.Commands.Delete;
using MiniERP.Application.Users.Validators;

namespace MiniERP.Application.Tests.Users.Validators
{
    public class DeleteUserCommandValidatorTests
    {
        private readonly DeleteUserCommandValidator _validator;

        public DeleteUserCommandValidatorTests()
        {
            _validator = new DeleteUserCommandValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenUserIdIsZero()
        {
            // Arrange
            var command = new DeleteUserCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId).WithErrorMessage("User ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenUserIdIsValid()
        {
            // Arrange
            var command = new DeleteUserCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
