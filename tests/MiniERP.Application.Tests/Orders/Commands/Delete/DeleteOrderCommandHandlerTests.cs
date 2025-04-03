using FluentAssertions;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Common.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Orders.Commands.Delete;
using MiniERP.Application.Users.Commands.Delete;
using MiniERP.Orders.Domain.Entities;

using Moq;

namespace MiniERP.Application.Tests.Orders.Commands.Delete;

public class DeleteOrderCommandHandlerTests
{
    private readonly Mock<IRepository<Order>> _orderRepositoryMock;
    private readonly Mock<IValidator<DeleteOrderCommand>> _validatorMock;
    private readonly DeleteOrderCommandHandler _handler;

    public DeleteOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IRepository<Order>>();
        _validatorMock = new Mock<IValidator<DeleteOrderCommand>>();
        _handler = new DeleteOrderCommandHandler(_orderRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenOrderIsDeleted()
    {
        // Arrange
        var command = new DeleteOrderCommand(1);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _orderRepositoryMock.Setup(r => r.DeleteAsync(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new Order { Id = command.OrderId }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new DeleteOrderCommand(1);
        var validationResult = new FluentValidation.Results.ValidationResult(
            new[] { new FluentValidation.Results.ValidationFailure("OrderId", "Validation error") });
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
        var command = new DeleteOrderCommand(1);
        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _orderRepositoryMock.Setup(r => r.DeleteAsync(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Repository error"));
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(command.OrderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(new Order { Id = command.OrderId }));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == "Repository error");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenOrderIsNotFound()
    {
        // Arrange
        var orderId = 1;
        var command = new DeleteOrderCommand(orderId);

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _orderRepositoryMock.Setup(r => r.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(ResultErrors.NotFound<Order>(1)));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<OrderNotFoundException>();
    }
}
