using FluentValidation;

using MiniERP.Application.Users.Queries.GetById;

namespace MiniERP.Application.Users.Validators;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("User ID must be greater than zero.");
    }
}
