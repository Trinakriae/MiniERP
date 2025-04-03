using FluentResults;

using FluentValidation;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;
using MiniERP.Application.Errors;
using MiniERP.Application.Extensions;

namespace MiniERP.Application.Addresses.Commands.Update;

public sealed class UpdateAddressCommandHandler(
    IRepository<Address> addressRepository,
    IMapper<Address, AddressDto> addressMapper,
    IValidator<UpdateAddressCommand> validator) : ICommandHandler<UpdateAddressCommand>
{
    private readonly IRepository<Address> _addressRepository = addressRepository
            ?? throw new ArgumentNullException(nameof(addressRepository));
    private readonly IMapper<Address, AddressDto> _addressMapper = addressMapper
            ?? throw new ArgumentNullException(nameof(addressMapper));
    private readonly IValidator<UpdateAddressCommand> _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result> Handle(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
        }

        var existingAddressResult = await _addressRepository.GetByIdAsync(command.AddressDto.Id.Value, cancellationToken);
        if (existingAddressResult.IsFailed && existingAddressResult.HasErrorCode(ErrorCodes.Address.NotFound))
        {
            throw new AddressNotFoundException(command.AddressDto.Id.Value);
        }

        var address = _addressMapper.Map(command.AddressDto);

        var updateResult = await _addressRepository.UpdateAsync(address, cancellationToken);
        if (updateResult.IsFailed)
        {
            return Result.Fail(updateResult.Errors);
        }

        return Result.Ok();
    }
}
