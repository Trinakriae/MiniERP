using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Commands.Delete;
using MiniERP.Application.Common.Errors;

using Moq;

namespace MiniERP.Application.Tests.Addresses.Commands.Delete;

public class DeleteAddressCommandHandlerTests
{
    private readonly Mock<IRepository<Address>> _mockAddressRepository;
    private readonly Mock<IValidator<DeleteAddressCommand>> _mockValidator;
    private readonly DeleteAddressCommandHandler _handler;

    public DeleteAddressCommandHandlerTests()
    {
        _mockAddressRepository = new Mock<IRepository<Address>>();
        _mockValidator = new Mock<IValidator<DeleteAddressCommand>>();
        _handler = new DeleteAddressCommandHandler(_mockAddressRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAddressIsDeleted()
    {
        // Arrange
        var command = new DeleteAddressCommand(1);
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAddressRepository.Setup(r => r.DeleteAsync(command.AddressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        _mockAddressRepository.Setup(r => r.GetByIdAsync(command.AddressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new Address { Id = command.AddressId }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new DeleteAddressCommand(1);
        var validationResult = new FluentValidation.Results.ValidationResult(
            [new FluentValidation.Results.ValidationFailure("AddressId", "Validation error")]);
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == "Validation error");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var command = new DeleteAddressCommand(1);
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAddressRepository.Setup(r => r.DeleteAsync(command.AddressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Repository error"));
        _mockAddressRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new Address { Id = command.AddressId }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == "Repository error");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenAddressIsNotFound()
    {
        // Arrange
        var addressId = 1;
        var command = new DeleteAddressCommand(addressId);

        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mockAddressRepository.Setup(r => r.GetByIdAsync(addressId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ResultErrors.NotFound<Address>(1)));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AddressNotFoundException>();
    }
}
