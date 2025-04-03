using FluentValidation.TestHelper;

using MiniERP.Application.Orders.Commands.Delete;
using MiniERP.Application.Orders.Validators;

namespace MiniERP.Application.Tests.Orders.Validators
{
    public class DeleteOrderCommandValidatorTests
    {
        private readonly DeleteOrderCommandValidator _validator;

        public DeleteOrderCommandValidatorTests()
        {
            _validator = new DeleteOrderCommandValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderIdIsZero()
        {
            // Arrange
            var command = new DeleteOrderCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderId).WithErrorMessage("Order ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenOrderIdIsValid()
        {
            // Arrange
            var command = new DeleteOrderCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.OrderId);
        }
    }
}
