using FluentValidation;

using MiniERP.Application.Orders.Queries.GetById;

namespace MiniERP.Application.Orders.Validators;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("Order ID must be greater than zero.");
    }
}
