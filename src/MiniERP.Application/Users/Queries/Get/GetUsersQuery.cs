using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Users.Dtos;

namespace MiniERP.Application.Users.Queries.Get;

public sealed record GetUsersQuery() : IQuery<IEnumerable<UserDto>>;



