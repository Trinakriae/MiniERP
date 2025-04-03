using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.Application.Addresses.Commands.Create;

public sealed record CreateAddressCommand(
    AddressDto AddressDto
) : ICommand<int>;