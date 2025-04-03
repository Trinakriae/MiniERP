using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Users.Dtos;

namespace MiniERP.Application.Users.Commands.Create
{
    public sealed record CreateUserCommand(
        UserDto UserDto
    ) : ICommand<int>;
}
