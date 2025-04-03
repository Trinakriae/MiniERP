using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Orders.Dtos;

namespace MiniERP.Application.Orders.Commands.Create
{
    public sealed record CreateOrderCommand(
        OrderDto OrderDto
    ) : ICommand<int>;
}