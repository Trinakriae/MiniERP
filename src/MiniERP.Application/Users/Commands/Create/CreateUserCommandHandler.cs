using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Users.Dtos;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Commands.Create
{
    public sealed class CreateUserCommandHandler(
        IRepository<User> userRepository,
        IMapper<User, UserDto> userMapper,
        IValidator<CreateUserCommand> validator) : ICommandHandler<CreateUserCommand, int>
    {
        private readonly IRepository<User> _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IMapper<User, UserDto> _userMapper = userMapper
                ?? throw new ArgumentNullException(nameof(userMapper));
        private readonly IValidator<CreateUserCommand> _validator = validator
                ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Result<int>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
            }

            var user = _userMapper.Map(command.UserDto);

            var addResult = await _userRepository.AddAsync(user, cancellationToken);
            if (addResult.IsFailed)
            {
                return Result.Fail(addResult.Errors);
            }

            return Result.Ok(addResult.Value);
        }
    }
}
