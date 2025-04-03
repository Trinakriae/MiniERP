using FluentResults;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Users.Dtos;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Queries.Get;

public sealed class GetUsersQueryHandler(
    IRepository<User> userRepository,
    IMapper<User, UserDto> userMapper) : IQueryHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IRepository<User> _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IMapper<User, UserDto> _userMapper = userMapper
            ?? throw new ArgumentNullException(nameof(userMapper));

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var usersResult = await _userRepository.GetAsync(cancellationToken);
        if (usersResult.IsFailed)
        {
            return Result.Fail(usersResult.Errors);
        }

        var userDtos = usersResult.Value.Select(user => _userMapper.Map(user));

        return Result.Ok(userDtos);
    }
}



