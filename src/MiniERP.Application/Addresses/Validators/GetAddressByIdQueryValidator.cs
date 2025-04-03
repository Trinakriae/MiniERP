using FluentValidation;

using MiniERP.Application.Addresses.Queries.GetById;

namespace MiniERP.Application.Addresses.Validators;

public class GetAddressByIdQueryValidator : AbstractValidator<GetAddressByIdQuery>
{
    public GetAddressByIdQueryValidator()
    {
        RuleFor(x => x.AddressId).GreaterThan(0).WithMessage("Address ID must be greater than zero.");
    }
}
