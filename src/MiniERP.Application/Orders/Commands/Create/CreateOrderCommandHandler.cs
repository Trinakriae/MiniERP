using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;

namespace MiniERP.Application.Orders.Commands.Create
{
    public sealed class CreateOrderCommandHandler(
        IRepository<Order> orderRepository,
        IMapper<Order, OrderDto> orderMapper,
        IValidator<CreateOrderCommand> validator) : ICommandHandler<CreateOrderCommand, int>
    {
        private readonly IRepository<Order> _orderRepository = orderRepository
                ?? throw new ArgumentNullException(nameof(orderRepository));
        private readonly IMapper<Order, OrderDto> _orderMapper = orderMapper
                ?? throw new ArgumentNullException(nameof(orderMapper));
        private readonly IValidator<CreateOrderCommand> _validator = validator
                ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Result<int>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
            }

            var order = _orderMapper.Map(command.OrderDto);

            var addResult = await _orderRepository.AddAsync(order, cancellationToken);
            if (addResult.IsFailed)
            {
                return Result.Fail(addResult.Errors);
            }

            return Result.Ok(addResult.Value);
        }
    }
}
