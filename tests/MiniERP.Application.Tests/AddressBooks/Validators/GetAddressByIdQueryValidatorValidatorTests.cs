using FluentValidation.TestHelper;

using MiniERP.Application.Addresses.Queries.GetById;
using MiniERP.Application.Addresses.Validators;

namespace MiniERP.Application.Tests.AddressBooks.Validators
{
    public class GetAddressByIdQueryValidatorValidatorTests
    {
        private readonly GetAddressByIdQueryValidator _validator;

        public GetAddressByIdQueryValidatorValidatorTests()
        {
            _validator = new GetAddressByIdQueryValidator();
        }

        [Fact]
        public async Task Should_HaveValidationError_WhenAddressIdIsZero()
        {
            // Arrange
            var query = new GetAddressByIdQuery(0);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AddressId).WithErrorMessage("Address ID must be greater than zero.");
        }

        [Fact]
        public async Task Should_NotHaveValidationError_WhenAddressIdIsValid()
        {
            // Arrange
            var query = new GetAddressByIdQuery(1);

            // Act
            var result = await _validator.TestValidateAsync(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.AddressId);
        }
    }
}
