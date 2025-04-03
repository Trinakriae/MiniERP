using FluentResults;

using FluentValidation;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Errors;
using MiniERP.Application.Extensions;

namespace MiniERP.Application.Addresses.Commands.Delete;

public sealed class DeleteAddressCommandHandler(
    IRepository<Address> addressRepository,
    IValidator<DeleteAddressCommand> validator) : ICommandHandler<DeleteAddressCommand>
{
    private readonly IRepository<Address> _addressRepository = addressRepository
            ?? throw new ArgumentNullException(nameof(addressRepository));
    private readonly IValidator<DeleteAddressCommand> _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result> Handle(DeleteAddressCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
        }

        var existingAddressResult = await _addressRepository.GetByIdAsync(command.AddressId, cancellationToken);
        if (existingAddressResult.IsFailed && existingAddressResult.HasErrorCode(ErrorCodes.Address.NotFound))
        {
            throw new AddressNotFoundException(command.AddressId);
        }


        return await _addressRepository.DeleteAsync(command.AddressId, cancellationToken);
    }
}
