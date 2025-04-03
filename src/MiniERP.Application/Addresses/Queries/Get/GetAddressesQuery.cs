using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.Application.Addresses.Queries.Get;

public sealed record GetAddressesQuery() : IQuery<IEnumerable<AddressDto>>;



