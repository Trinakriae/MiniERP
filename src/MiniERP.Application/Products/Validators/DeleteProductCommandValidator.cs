using FluentValidation;

using MiniERP.Application.Products.Commands.Delete;

namespace MiniERP.Application.Products.Validators
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Product ID must be greater than zero.");
        }
    }
}
