using FluentResults;

using FluentValidation;

using MediatR;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Errors;
using MiniERP.Application.Extensions;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Orders.Commands.Delete;

public sealed class DeleteOrderCommandHandler(
    IRepository<Order> orderRepository,
    IValidator<DeleteOrderCommand> validator) : IRequestHandler<DeleteOrderCommand, Result>
{
    private readonly IRepository<Order> _orderRepository = orderRepository
            ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IValidator<DeleteOrderCommand> _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
        }

        var existingOrderResult = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (existingOrderResult.IsFailed && existingOrderResult.HasErrorCode(ErrorCodes.Order.NotFound))
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        return await _orderRepository.DeleteAsync(command.OrderId, cancellationToken);
    }
}
