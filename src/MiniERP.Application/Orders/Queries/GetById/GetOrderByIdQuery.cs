using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Orders.Dtos;

namespace MiniERP.Application.Orders.Queries.GetById
{
    public sealed record GetOrderByIdQuery(
        int OrderId
    ) : IQuery<OrderDto>;
}
