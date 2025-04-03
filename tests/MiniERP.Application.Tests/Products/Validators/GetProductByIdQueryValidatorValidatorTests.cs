using FluentValidation.TestHelper;

using MiniERP.Application.Products.Queries.GetById;
using MiniERP.Application.Products.Validators;

namespace MiniERP.Application.Tests.Products.Validators
{
    public class GetProductByIdQueryValidatorValidatorTests
    {
        private readonly GetProductByIdQueryValidator _validator;

        public GetProductByIdQueryValidatorValidatorTests()
        {
            _validator = new GetProductByIdQueryValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenProductIdIsZero()
        {
            // Arrange
            var query = new GetProductByIdQuery(0);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductId).WithErrorMessage("Product ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenProductIdIsValid()
        {
            // Arrange
            var query = new GetProductByIdQuery(1);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProductId);
        }
    }
}
