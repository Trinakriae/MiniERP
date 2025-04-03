using MiniERP.Application.Exceptions;

public class AddressNotFoundException : NotFoundException
{
    public AddressNotFoundException(int addressId)
        : base($"Address with ID {addressId} was not found.") { }
}