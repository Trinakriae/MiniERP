using FluentValidation.TestHelper;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Update;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Addresses.Validators;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.AddressBooks.Validators;

public class UpdateAddressValidatorTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IRepository<Address>> _mockAddressRepository;
    private readonly UpdateAddressCommandValidator _validator;

    public UpdateAddressValidatorTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockAddressRepository = new Mock<IRepository<Address>>();
        _validator = new UpdateAddressCommandValidator(_mockUserRepository.Object, _mockAddressRepository.Object);
    }

    [Fact]
    public async Task Should_HaveValidationError_WhenAddressIsNull()
    {
        // Arrange
        UpdateAddressCommand? command = new(null as AddressDto);

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
        var command = new UpdateAddressCommand(addressDto);

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
        var command = new UpdateAddressCommand(addressDto);

        _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddressDto.User.Id).WithErrorMessage("User does not exist.");
    }

    [Fact]
    public async Task Should_HaveValidationError_WhenAddressDoesNotExist()
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
        var command = new UpdateAddressCommand(addressDto);

        _mockAddressRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AddressDto.Id.Value).WithErrorMessage("Address does not exist.");
    }

    [Fact]
    public async Task Should_NotHaveValidationError_WhenAddressIsValid()
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
        var command = new UpdateAddressCommand(addressDto);

        _mockUserRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _mockAddressRepository.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


