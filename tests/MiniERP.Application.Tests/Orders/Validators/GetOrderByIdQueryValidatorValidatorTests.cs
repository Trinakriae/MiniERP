using FluentValidation.TestHelper;

using MiniERP.Application.Orders.Queries.GetById;
using MiniERP.Application.Orders.Validators;

namespace MiniERP.Application.Tests.Orders.Validators
{
    public class GetOrderByIdQueryValidatorValidatorTests
    {
        private readonly GetOrderByIdQueryValidator _validator;

        public GetOrderByIdQueryValidatorValidatorTests()
        {
            _validator = new GetOrderByIdQueryValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenOrderIdIsZero()
        {
            // Arrange
            var query = new GetOrderByIdQuery(0);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.OrderId).WithErrorMessage("Order ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenOrderIdIsValid()
        {
            // Arrange
            var query = new GetOrderByIdQuery(1);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.OrderId);
        }
    }
}
