using MiniERP.Application.Abstractions.Messaging;

namespace MiniERP.Application.Users.Commands.Update
{
    public sealed record UpdateUserCommand(
        Dtos.UserDto UserDto
    ) : ICommand;
}

