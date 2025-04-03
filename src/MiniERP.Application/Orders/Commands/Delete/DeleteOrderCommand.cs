using FluentResults;

using MediatR;

namespace MiniERP.Application.Orders.Commands.Delete;

public sealed record DeleteOrderCommand(int OrderId) : IRequest<Result>;
