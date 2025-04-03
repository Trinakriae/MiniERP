using FluentValidation.TestHelper;

using MiniERP.Application.Products.Commands.Delete;
using MiniERP.Application.Products.Validators;

namespace MiniERP.Application.Tests.Products.Validators
{
    public class DeleteProductValidatorTests
    {
        private readonly DeleteProductCommandValidator _validator;

        public DeleteProductValidatorTests()
        {
            _validator = new DeleteProductCommandValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductIdIsZero()
        {
            // Arrange
            var command = new DeleteProductCommand(0);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductId).WithErrorMessage("Product ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenProductIdIsValid()
        {
            // Arrange
            var command = new DeleteProductCommand(1);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
        }
    }
}
