using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Users.Commands.Delete;
using MiniERP.Users.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Users.Commands.Delete;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IValidator<DeleteUserCommand>> _validatorMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IRepository<User>>();
        _validatorMock = new Mock<IValidator<DeleteUserCommand>>();
        _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserIsDeleted()
    {
        // Arrange
        var command = new DeleteUserCommand(1);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _userRepositoryMock.Setup(r => r.DeleteAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        _userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new User { Id = command.UserId }));


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new DeleteUserCommand(1);
        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("UserId", "Validation error") });
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
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
        var command = new DeleteUserCommand(1);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _userRepositoryMock.Setup(r => r.DeleteAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Repository error"));
        _userRepositoryMock.Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new User { Id = command.UserId }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == "Repository error");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserIsNotFound()
    {
        // Arrange
        var userId = 1;
        var command = new DeleteUserCommand(userId);

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ResultErrors.NotFound<User>(1)));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}
