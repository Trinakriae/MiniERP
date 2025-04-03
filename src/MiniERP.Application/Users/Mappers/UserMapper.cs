using MiniERP.Application.Abstractions;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Users.Mappers
{
    public class UserMapper : IMapper<User, Dtos.UserDto>
    {
        public User Map(Dtos.UserDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            return new User
            {
                Id = dto.Id ?? default,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public Dtos.UserDto Map(User entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new Dtos.UserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };
        }
    }
}
