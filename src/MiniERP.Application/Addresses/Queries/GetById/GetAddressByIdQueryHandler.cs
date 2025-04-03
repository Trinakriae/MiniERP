using FluentResults;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Users.Domain.Entities;

namespace MiniERP.Application.Addresses.Queries.GetById;

public sealed class GetAddressByIdQueryHandler(
    IRepository<Address> addressRepository,
    IRepository<User> userRepository,
    IMapper<Address, AddressDto> addressMapper,
    IMapper<User, AddressUserDto> userMapper) : IQueryHandler<GetAddressByIdQuery, AddressDto>
{
    private readonly IRepository<Address> _addressRepository = addressRepository
            ?? throw new ArgumentNullException(nameof(addressRepository));
    private readonly IRepository<User> _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IMapper<Address, AddressDto> _addressMapper = addressMapper
            ?? throw new ArgumentNullException(nameof(addressMapper));
    private readonly IMapper<User, AddressUserDto> _userMapper = userMapper
            ?? throw new ArgumentNullException(nameof(userMapper));

    public async Task<Result<AddressDto>> Handle(GetAddressByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var addressResult = await _addressRepository.GetByIdAsync(query.AddressId, cancellationToken);
        if (addressResult.IsFailed)
        {
            return Result.Fail(addressResult.Errors);
        }

        var address = addressResult.Value;
        var userResult = await _userRepository.GetByIdAsync(address.UserId, cancellationToken);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }

        var addressDto = _addressMapper.Map(address);
        addressDto.User = _userMapper.Map(userResult.Value);

        return Result.Ok(addressDto);
    }
}




