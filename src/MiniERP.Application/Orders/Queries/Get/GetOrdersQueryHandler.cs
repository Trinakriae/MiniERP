using FluentResults;

using MediatR;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Orders.Domain.Entities;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Orders.Queries.Get;

public sealed class GetOrdersQueryHandler(
    IRepository<Order> orderRepository,
    IRepository<User> userRepository,
    IMapper<Order, OrderDto> orderMapper,
    IMapper<User, OrderUserDto> userMapper) : IRequestHandler<GetOrdersQuery, Result<IEnumerable<OrderDto>>>
{
    private readonly IRepository<Order> _orderRepository = orderRepository
            ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IRepository<User> _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IMapper<Order, OrderDto> _orderMapper = orderMapper
            ?? throw new ArgumentNullException(nameof(orderMapper));
    private readonly IMapper<User, OrderUserDto> _userMapper = userMapper
            ?? throw new ArgumentNullException(nameof(userMapper));

    public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var ordersResult = await _orderRepository.GetAsync(cancellationToken);
        if (ordersResult.IsFailed)
        {
            return Result.Fail(ordersResult.Errors);
        }

        var orderDtos = new List<OrderDto>();

        foreach (var order in ordersResult.Value)
        {
            var orderDto = _orderMapper.Map(order);

            var userResult = await _userRepository.GetByIdAsync(orderDto.User.Id, cancellationToken);
            if (userResult.IsFailed)
            {
                return Result.Fail(userResult.Errors);
            }

            orderDto.User = _userMapper.Map(userResult.Value);
            orderDtos.Add(orderDto);
        }

        return Result.Ok(orderDtos.AsEnumerable());
    }
}
