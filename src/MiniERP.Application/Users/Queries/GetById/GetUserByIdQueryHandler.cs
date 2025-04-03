using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Users.Dtos;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Queries.GetById;

public sealed class GetUserByIdQueryHandler(
    IRepository<User> userRepository,
    IMapper<User, UserDto> userMapper) : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IRepository<User> _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IMapper<User, UserDto> _userMapper = userMapper
            ?? throw new ArgumentNullException(nameof(userMapper));

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var userResult = await _userRepository.GetByIdAsync(query.UserId, cancellationToken);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        var userDto = _userMapper.Map(userResult.Value);

        return Result.Ok(userDto);
    }
}



