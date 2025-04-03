using FluentValidation.TestHelper;

using MiniERP.Application.Users.Queries.GetById;
using MiniERP.Application.Users.Validators;

namespace MiniERP.Application.Tests.Users.Validators
{
    public class GetUserByIdQueryValidatorValidatorTests
    {
        private readonly GetUserByIdQueryValidator _validator;

        public GetUserByIdQueryValidatorValidatorTests()
        {
            _validator = new GetUserByIdQueryValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenUserIdIsZero()
        {
            // Arrange
            var query = new GetUserByIdQuery(0);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId).WithErrorMessage("User ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenUserIdIsValid()
        {
            // Arrange
            var query = new GetUserByIdQuery(1);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
