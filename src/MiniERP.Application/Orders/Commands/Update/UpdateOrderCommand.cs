using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Orders.Dtos;

namespace MiniERP.Application.Orders.Commands.Update
{
    public sealed record UpdateOrderCommand(
            OrderDto OrderDto
        ) : ICommand;
}
