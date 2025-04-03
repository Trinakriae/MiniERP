using FluentValidation.TestHelper;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Create;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Addresses.Validators;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.AddressBooks.Validators;

public class CreateAddressValidatorTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly CreateAddressCommandValidator _validator;

    public CreateAddressValidatorTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _validator = new CreateAddressCommandValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task Should_HaveValidationError_WhenAddressIsNull()
    {
        // Arrange
        CreateAddressCommand? command = new(null as AddressDto);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddressDto).WithErrorMessage("Address must not be null.");
    }

    [Fact]
    public async Task Should_HaveValidationError_WhenStreetIsEmpty()
    {
        // Arrange
        var addressDto = new AddressDto
        {
            Id = 1,
            Street = string.Empty,
            City = "Test City",
            State = "Test State",
            PostalCode = "12345",
            Country = "Test Country",
            User = new AddressUserDto { Id = 1 }
        };
        var command = new CreateAddressCommand(addressDto);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddressDto.Street).WithErrorMessage("Street must not be empty.");
    }

    [Fact]
    public async Task Should_HaveValidationError_WhenUserDoesNotExist()
    {
        // Arrange
        var addressDto = new AddressDto
        {
            Id = 1,
            Street = "123 Main St",
            City = "Test City",
            State = "Test State",
            PostalCode = "12345",
            Country = "Test Country",
            User = new AddressUserDto { Id = 1 }
        };
        var command = new CreateAddressCommand(addressDto);

        _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddressDto.User.Id)
            .WithErrorMessage("User does not exist.");
    }

    [Fact]
    public async Task Should_NotHaveValidationError_WhenAddressIsValid()
    {
        // Arrange
        var addressDto = new AddressDto
        {
            Street = "123 Main St",
            City = "Test City",
            State = "Test State",
            PostalCode = "12345",
            Country = "Test Country",
            User = new AddressUserDto { Id = 1 }
        };
        var command = new CreateAddressCommand(addressDto);

        _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


