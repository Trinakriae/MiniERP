using FluentResults;

using FluentValidation;

using MiniERP.Application.Abstractions;
using MiniERP.Application.Abstractions.Messaging;
using MiniERP.Application.Errors;
using MiniERP.Application.Extensions;
using MiniERP.Application.Products.Dtos;
using MiniERP.Products.Domain.Entities;

namespace MiniERP.Application.Products.Commands.Update
{
    public sealed class UpdateProductCommandHandler(
        IRepository<Product> productRepository,
        IMapper<Product, ProductDto> productMapper,
        IValidator<UpdateProductCommand> validator) : ICommandHandler<UpdateProductCommand>
    {
        private readonly IRepository<Product> _productRepository = productRepository
                ?? throw new ArgumentNullException(nameof(productRepository));
        private readonly IMapper<Product, ProductDto> _productMapper = productMapper
                ?? throw new ArgumentNullException(nameof(productMapper));
        private readonly IValidator<UpdateProductCommand> _validator = validator
                ?? throw new ArgumentNullException(nameof(validator));

        public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));
            }

            var existingProductResult = await _productRepository.GetByIdAsync(command.ProductDto.Id.Value, cancellationToken);
            if (existingProductResult.IsFailed && existingProductResult.HasErrorCode(ErrorCodes.Product.NotFound))
            {
                throw new ProductNotFoundException(command.ProductDto.Id.Value);
            }

            var product = _productMapper.Map(command.ProductDto);

            var updateResult = await _productRepository.UpdateAsync(product, cancellationToken);
            if (updateResult.IsFailed)
            {
                return Result.Fail(updateResult.Errors);
            }

            return Result.Ok();
        }
    }
}
