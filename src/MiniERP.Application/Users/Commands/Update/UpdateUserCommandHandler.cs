using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Errors;
using MiniERP.Application.Exceptions;
using MiniERP.Application.Extensions;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Commands.Update
{
    public sealed class UpdateUserCommandHandler(
        IRepository<User> userRepository,
        IMapper<User, Dtos.UserDto> userMapper,
        IValidator<UpdateUserCommand> validator) : ICommandHandler<UpdateUserCommand>
    {
        private readonly IRepository<User> _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IMapper<User, Dtos.UserDto> _userMapper = userMapper
                ?? throw new ArgumentNullException(nameof(userMapper));
        private readonly IValidator<UpdateUserCommand> _validator = validator
                ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
            }

            var existingUserResult = await _userRepository.GetByIdAsync(command.UserDto.Id.Value, cancellationToken);
            if (existingUserResult.IsFailed && existingUserResult.HasErrorCode(ErrorCodes.User.NotFound))
            {
                throw new UserNotFoundException(command.UserDto.Id.Value);
            }

            var user = _userMapper.Map(command.UserDto);

            var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);
            if (updateResult.IsFailed)
            {
                return Result.Fail(updateResult.Errors);
            }

            return Result.Ok();
        }
    }
}

