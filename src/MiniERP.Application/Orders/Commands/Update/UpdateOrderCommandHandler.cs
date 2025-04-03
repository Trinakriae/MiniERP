using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Errors;
using MiniERP.Application.Extensions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Orders.Commands.Update
{
    public sealed class UpdateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IMapper<Order, OrderDto> orderMapper,
        IValidator<UpdateOrderCommand> validator) : ICommandHandler<UpdateOrderCommand>
    {
        private readonly IRepository<Order> _orderRepository = orderRepository
                ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly IMapper<Order, OrderDto> _orderMapper = orderMapper
                ?? throw new ArgumentNullException(nameof(orderMapper));
        private readonly IValidator<UpdateOrderCommand> _validator = validator
                ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Result> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
            }

            var existingOrderResult = await _orderRepository.GetByIdAsync(command.OrderDto.Id.Value, cancellationToken);
            if (existingOrderResult.IsFailed && existingOrderResult.HasErrorCode(ErrorCodes.Order.NotFound))
            {
                throw new OrderNotFoundException(command.OrderDto.Id.Value);
            }

            var updatedOrder = _orderMapper.Map(command.OrderDto);

            var updateResult = await _orderRepository.UpdateAsync(updatedOrder, cancellationToken);
            if (updateResult.IsFailed)
            {
                return Result.Fail(updateResult.Errors);
            }

            return Result.Ok();
        }
    }
}
