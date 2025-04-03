using FluentValidation.TestHelper;

using MiniERP.Application.Addresses.Commands.Delete;
using MiniERP.Application.Addresses.Validators;

namespace MiniERP.Application.Tests.AddressBooks.Validators
{
    public class DeleteAddressCommandValidatorTests
    {
        private readonly DeleteAddressCommandValidator _validator;

        public DeleteAddressCommandValidatorTests()
        {
            _validator = new DeleteAddressCommandValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenAddressIdIsZero()
        {
            // Arrange
            var command = new DeleteAddressCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AddressId).WithErrorMessage("Address ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenAddressIdIsValid()
        {
            // Arrange
            var command = new DeleteAddressCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.AddressId);
        }
    }
}
