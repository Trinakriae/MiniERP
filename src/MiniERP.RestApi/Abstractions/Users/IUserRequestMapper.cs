using MiniERP.Application.Users.Dtos;

namespace MiniERP.RestApi.Abstractions.Users;

public interface IUserRequestMapper<TRequest>
{
    UserDto MapToUserDto(TRequest request);
}


