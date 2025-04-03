using FluentResults;

using MediatR;

using MiniERP.Application.Orders.Dtos;

namespace MiniERP.Application.Orders.Queries.Get;

public sealed record GetOrdersQuery() : IRequest<Result<IEnumerable<OrderDto>>>;
