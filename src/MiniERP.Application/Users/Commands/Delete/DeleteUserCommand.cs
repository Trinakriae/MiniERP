using MiniERP.Application.Abstractions.Messaging;

namespace MiniERP.Application.Users.Commands.Delete;

public sealed record DeleteUserCommand(int UserId) : ICommand;
