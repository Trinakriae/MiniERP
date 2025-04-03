using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Update;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Common.Errors;

using Moq;

namespace MiniERP.Application.Tests.AddressBooks.Commands.Update;

public class UpdateAddressCommandHandlerTests
{
    private readonly Mock<IRepository<Address>> _mockAddressRepository;
    private readonly Mock<IMapper<Address, AddressDto>> _mockAddressMapper;
    private readonly Mock<IValidator<UpdateAddressCommand>> _mockValidator;
    private readonly UpdateAddressCommandHandler _handler;

    public UpdateAddressCommandHandlerTests()
    {
        _mockAddressRepository = new Mock<IRepository<Address>>();
        _mockAddressMapper = new Mock<IMapper<Address, AddressDto>>();
        _mockValidator = new Mock<IValidator<UpdateAddressCommand>>();
        _handler = new UpdateAddressCommandHandler(
            _mockAddressRepository.Object,
            _mockAddressMapper.Object,
            _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnOk_WhenAddressIsValid()
    {
        // Arrange
        var addressDto = new AddressDto { Id = 1, Street = "123 Main St", City = "Test City", State = "Test State", PostalCode = "12345", Country = "Test Country", User = new AddressUserDto { Id = 1 } };
        var address = new Address { Id = 1, Street = "123 Main St", City = "Test City", State = "Test State", PostalCode = "12345", Country = "Test Country", UserId = 1 };
        var command = new UpdateAddressCommand(addressDto);

        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mockAddressMapper.Setup(m => m.Map(addressDto)).Returns(address);
        _mockAddressRepository.Setup(r => r.UpdateAsync(address, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        _mockAddressRepository.Setup(r => r.GetByIdAsync(addressDto.Id.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(address));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFail_WhenAddressIsInvalid()
    {
        // Arrange
        var addressDto = new AddressDto { Id = 1, Street = "123 Main St", City = "Test City", State = "Test State", PostalCode = "12345", Country = "Test Country", User = new AddressUserDto { Id = 1 } };
        var command = new UpdateAddressCommand(addressDto);

        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(
            [
                new ValidationFailure("AddressDto.Street", "Street must not be empty.")
            ]));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenAddressIsNotFound()
    {
        // Arrange
        var addressDto = new AddressDto { Id = 1 };
        var command = new UpdateAddressCommand(addressDto);

        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mockAddressRepository.Setup(r => r.GetByIdAsync(addressDto.Id.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ResultErrors.NotFound<Address>(addressDto.Id.Value)));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AddressNotFoundException>();
    }
}


