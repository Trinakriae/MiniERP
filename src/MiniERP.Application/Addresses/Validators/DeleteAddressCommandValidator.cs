using FluentValidation;

using MiniERP.Application.Addresses.Commands.Delete;

namespace MiniERP.Application.Addresses.Validators;

public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressCommandValidator()
    {
        RuleFor(x => x.AddressId).GreaterThan(0).WithMessage("Address ID must be greater than zero.");
    }
}
