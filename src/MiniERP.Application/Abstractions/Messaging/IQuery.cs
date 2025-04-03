using FluentResults;

using MediatR;

namespace MiniERP.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
