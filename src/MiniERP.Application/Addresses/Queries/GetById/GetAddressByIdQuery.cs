using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.Application.Addresses.Queries.GetById;

public sealed record GetAddressByIdQuery(
    int AddressId
) : IQuery<AddressDto>;




