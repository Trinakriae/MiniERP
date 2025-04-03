using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Orders.Queries.GetById;

public sealed class GetOrderByIdQueryHandler(
    IRepository<Order> orderRepository,
    IRepository<User> userRepository,
    IMapper<Order, OrderDto> orderMapper,
    IMapper<User, OrderUserDto> userMapper) : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IRepository<Order> _orderRepository = orderRepository
            ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IRepository<User> _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IMapper<Order, OrderDto> _orderMapper = orderMapper
            ?? throw new ArgumentNullException(nameof(orderMapper));
    private readonly IMapper<User, OrderUserDto> _userMapper = userMapper
            ?? throw new ArgumentNullException(nameof(userMapper));

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var orderResult = await _orderRepository.GetByIdAsync(query.OrderId, cancellationToken);
        if (orderResult.IsFailed)
        {
            return Result.Fail(orderResult.Errors);
        }

        var orderDto = _orderMapper.Map(orderResult.Value);

        var userResult = await _userRepository.GetByIdAsync(orderDto.User.Id, cancellationToken);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        orderDto.User = _userMapper.Map(userResult.Value);

        return Result.Ok(orderDto);
    }
}
