using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Commands.Create;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CreateProductCommandValidator(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository
                ?? throw new ArgumentNullException(nameof(categoryRepository));

            RuleFor(x => x.ProductDto).NotNull().WithMessage("Product must not be null.");

            When(x => x.ProductDto != null, () =>
            {
                RuleFor(x => x.ProductDto.Id).Null().WithMessage("Product ID must be empty.");
                RuleFor(x => x.ProductDto.Name).NotEmpty().WithMessage("Product name must not be empty.");
                RuleFor(x => x.ProductDto.UnitPrice).GreaterThan(0).WithMessage("Product price must be greater than zero.");
                RuleFor(x => x.ProductDto.Category).NotNull().WithMessage("Product must have a category.");
                RuleFor(x => x.ProductDto.Category.Id)
                    .MustAsync(CategoryExists)
                    .WithMessage("Category does not exist.")
                    .When(x => x.ProductDto.Category != null);
            });
        }

        private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
        {
            return await _categoryRepository.ExistsAsync(categoryId, cancellationToken);
        }
    }
}
