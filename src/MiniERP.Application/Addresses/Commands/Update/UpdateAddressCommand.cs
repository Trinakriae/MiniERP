using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.Application.Addresses.Commands.Update;

public sealed record UpdateAddressCommand(
    AddressDto AddressDto
) : ICommand;
