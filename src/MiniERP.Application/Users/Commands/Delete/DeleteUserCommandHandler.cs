using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Extensions;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Commands.Delete;

public sealed class DeleteUserCommandHandler(
    IRepository<User> userRepository,
    IValidator<DeleteUserCommand> validator) : ICommandHandler<DeleteUserCommand>
{
    private readonly IRepository<User> _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IValidator<DeleteUserCommand> _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
        }

        var existingUserResult = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (existingUserResult.IsFailed && existingUserResult.HasErrorCode(ErrorCodes.User.NotFound))
        {
            throw new UserNotFoundException(command.UserId);
        }

        return await _userRepository.DeleteAsync(command.UserId, cancellationToken);
    }
}
