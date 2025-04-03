using MiniERP.Application.Abstractions;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Addresses.Mappers
{
    public class UserToAddressUserDtoMapper : IMapper<User, AddressUserDto>
    {
        public AddressUserDto Map(User user)
        {
            if (user == null)
            {
                return null;
            }
            return new AddressUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public User Map(AddressUserDto userSummaryDto)
        {
            if (userSummaryDto == null)
            {
                return null;
            }
            return new User
            {
                Id = userSummaryDto.Id,
                FirstName = userSummaryDto.FirstName,
                LastName = userSummaryDto.LastName
            };
        }
    }
}
