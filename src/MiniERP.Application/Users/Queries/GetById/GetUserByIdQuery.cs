using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Users.Dtos;

namespace MiniERP.Application.Users.Queries.GetById;

public sealed record GetUserByIdQuery(
    int UserId
) : IQuery<UserDto>;



