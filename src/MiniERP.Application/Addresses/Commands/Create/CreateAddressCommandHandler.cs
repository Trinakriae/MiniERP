using FluentResults;

using FluentValidation;

using MiniERP.AddressBook.Domain.Entities;
using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Addresses.Dtos;

namespace MiniERP.Application.Addresses.Commands.Create;

public sealed class CreateAddressCommandHandler(
    IRepository<Address> addressRepository,
    IMapper<Address, AddressDto> addressMapper,
    IValidator<CreateAddressCommand> validator) : ICommandHandler<CreateAddressCommand, int>
{
    private readonly IRepository<Address> _addressRepository = addressRepository
            ?? throw new ArgumentNullException(nameof(addressRepository));
    private readonly IMapper<Address, AddressDto> _addressMapper = addressMapper
            ?? throw new ArgumentNullException(nameof(addressMapper));
    private readonly IValidator<CreateAddressCommand> _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result<int>> Handle(CreateAddressCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
        }

        var address = _addressMapper.Map(command.AddressDto);

        var addResult = await _addressRepository.AddAsync(address, cancellationToken);
        if (addResult.IsFailed)
        {
            return Result.Fail(addResult.Errors);
        }

        return Result.Ok(addResult.Value);
    }
}
