using FluentValidation;

using MiniERP.Application.Products.Queries.GetById;

namespace MiniERP.Application.Products.Validators;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product ID must be greater than zero.");
    }
}
