using MiniERP.Application.Abstractions.Messaging;

namespace MiniERP.Application.Addresses.Commands.Delete
{
    public sealed record DeleteAddressCommand(int AddressId) : ICommand;
}
