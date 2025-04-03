using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Create;
using MiniERP.Application.Addresses.Dtos;

using Moq;

namespace MiniERP.Application.Tests.AddressBooks.Commands.Create;

public class CreateAddressCommandHandlerTests
{
    private readonly Mock<IRepository<Address>> _mockAddressRepository;
    private readonly Mock<IMapper<Address, AddressDto>> _mockAddressMapper;
    private readonly Mock<IValidator<CreateAddressCommand>> _mockValidator;
    private readonly CreateAddressCommandHandler _handler;

    public CreateAddressCommandHandlerTests()
    {
        _mockAddressRepository = new Mock<IRepository<Address>>();
        _mockAddressMapper = new Mock<IMapper<Address, AddressDto>>();
        _mockValidator = new Mock<IValidator<CreateAddressCommand>>();
        _handler = new CreateAddressCommandHandler(
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
        var command = new CreateAddressCommand(addressDto);

        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mockAddressMapper.Setup(m => m.Map(addressDto))
            .Returns(address);
        _mockAddressRepository.Setup(r => r.AddAsync(address, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

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
        var command = new CreateAddressCommand(addressDto);

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
}


