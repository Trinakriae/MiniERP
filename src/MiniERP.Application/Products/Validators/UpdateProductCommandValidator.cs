using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Products.Commands.Update;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;

        public UpdateProductCommandValidator(IRepository<Category> categoryRepository, IRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository
                ?? throw new ArgumentNullException(nameof(categoryRepository));
            _productRepository = productRepository
                ?? throw new ArgumentNullException(nameof(productRepository));

            RuleFor(x => x.ProductDto).NotNull().WithMessage("Product must not be null.");

            When(x => x.ProductDto != null, () =>
            {
                RuleFor(x => x.ProductDto.Id).NotNull().WithMessage("Product ID must not be null.");
                RuleFor(x => x.ProductDto.Id.Value)
                    .MustAsync(ProductExists)
                    .WithMessage("Product does not exist.")
                    .When(x => x.ProductDto.Id.HasValue);
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

        private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
        {
            return await _productRepository.ExistsAsync(productId, cancellationToken);
        }
    }
}
