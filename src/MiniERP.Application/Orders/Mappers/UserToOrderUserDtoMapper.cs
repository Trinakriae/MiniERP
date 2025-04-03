using MiniERP.Application.Abstractions;
using MiniERP.Application.Orders.Dtos;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Orders.Mappers
{
    public class UserToOrderUserDtoMapper : IMapper<User, OrderUserDto>
    {
        public OrderUserDto Map(User user)
        {
            if (user == null)
            {
                return null;
            }
            return new OrderUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public User Map(OrderUserDto userSummaryDto)
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
