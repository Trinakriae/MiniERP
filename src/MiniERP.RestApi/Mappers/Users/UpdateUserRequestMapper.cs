using MiniERP.Application.Users.Dtos;
using MiniERP.RestApi.Abstractions.Users;
using MiniERP.RestApi.Models.Users;

namespace MiniERP.RestApi.Mappers.Users;

public class UpdateUserRequestMapper : IUserRequestMapper<UpdateUserRequest>
{
    public UserDto MapToUserDto(UpdateUserRequest request) => new()
    {
        Id = request.Id,
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
        PhoneNumber = request.PhoneNumber
    };
}


