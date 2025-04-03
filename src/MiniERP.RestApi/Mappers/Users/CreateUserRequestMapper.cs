using MiniERP.Application.Users.Dtos;
using MiniERP.RestApi.Abstractions.Users;
using MiniERP.RestApi.Models.Users;

namespace MiniERP.RestApi.Mappers.Users;

public class CreateUserRequestMapper : IUserRequestMapper<CreateUserRequest>
{
    public UserDto MapToUserDto(CreateUserRequest request) => new()
    {
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber
    };
}


